using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.Users.Application.UseCases.Commands;

namespace SocialMedia.Users.Endpoints;

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
		var files = HttpContext.Request.Form.Files;

		if (files.Count == 0)
		{
			await Send.ErrorsAsync(400, ct);
		}

		var fileReads = files.Select(async file =>
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

		var result = await mediator.Send(new UploadProfileImageCommand(req.Model, fileDtos.FirstOrDefault()), ct);

		if (result.IsNoContent())
		{
			await Send.NoContentAsync(ct);
		}
		else if (result.IsInvalid())
		{
			await Send.ErrorsAsync(400, ct);
		}
	}
}