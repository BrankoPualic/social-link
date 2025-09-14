using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.UseCases.Commands;

namespace SocialLink.Users.Endpoints;

[Authorize]
internal class UploadProfileImage(IMediator mediator) : Endpoint<UploadProfileImageRequest>
{
	public override void Configure()
	{
		Post("/users/uploadProfileImage");
		AllowFileUploads();
	}

	public override async Task HandleAsync(UploadProfileImageRequest req, CancellationToken ct)
	{
		if (Files.Count is 0)
		{
			await Send.ErrorsAsync(400, ct);
		}

		var fileReads = Files.Select(async file =>
		{
			using var stream = new MemoryStream();
			await file.CopyToAsync(stream);
			return new FileInformationDto(
				file.FileName,
				file.ContentType,
				stream.ToArray(),
				file.Length
			);
		});

		var fileDtos = await Task.WhenAll(fileReads);

		var result = await mediator.Send(new UploadProfileImageCommand(req.UserId, fileDtos.FirstOrDefault()), ct);

		await result.SendResponseAsync(HttpContext, ct);
	}
}