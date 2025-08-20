using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Commands;

namespace SocialLink.Users.Endpoints;

internal class Signup(IMediator mediator) : Endpoint<SignupDto, TokenDto>
{
	public override void Configure()
	{
		Post("/users/signup");
		AllowAnonymous();
		AllowFileUploads();
	}

	public override async Task HandleAsync(SignupDto req, CancellationToken ct)
	{
		var files = HttpContext.Request.Form.Files;

		var fileReads = files?.Select(async file =>
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

		var result = await mediator.Send(new SignupCommand(req, fileDtos.FirstOrDefault()), ct);

		await result.SendResponseAsync(HttpContext, ct: ct);
	}
}