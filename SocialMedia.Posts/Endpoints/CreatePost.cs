using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.Posts.Application.UseCases.Commands;

namespace SocialMedia.Posts.Endpoints;

[Authorize]
internal class CreatePost(IMediator mediator) : Endpoint<CreatePostRequest>
{
	public override void Configure()
	{
		Post("/posts/create");
		AllowFileUploads();
	}

	public override async Task HandleAsync(CreatePostRequest req, CancellationToken ct)
	{
		var files = HttpContext.Request.Form.Files;

		var filesReads = files?.Select(async file =>
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

		var fileDtos = await Task.WhenAll(filesReads);

		var result = await mediator.Send(new CreatePostCommand(req.Model, fileDtos.ToList()), ct);

		if (result.IsCreated())
		{
			await Send.OkAsync(result, ct);
		}
		else if (result.IsInvalid())
		{
			await Send.ErrorsAsync(400, ct);
		}
		else if (result.IsError())
		{
			await Send.ErrorsAsync(500, ct);
		}
	}
}
