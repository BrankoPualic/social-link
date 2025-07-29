using Ardalis.Result;
using FastEndpoints;
using MediatR;
using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Application.UseCases.Commands;

namespace SocialMedia.Users.Endpoints;

internal class Signup(IMediator mediator) : Endpoint<SignupRequest, TokenDto>
{
	public override void Configure()
	{
		Post("/users/signup");
		AllowAnonymous();
		AllowFileUploads();
	}

	public override async Task HandleAsync(SignupRequest req, CancellationToken ct)
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

		var result = await mediator.Send(new SignupCommand(req.Model, fileDtos.FirstOrDefault()), ct);

		if (result.IsOk())
		{
			await Send.OkAsync(result, ct);
		}
		else if (result.IsInvalid())
		{
			await Send.ErrorsAsync(cancellation: ct);
		}
	}
}