using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Application.UseCases.Commands;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Endpoints;

[Authorize]
internal class CreatePost(IMediator mediator) : Endpoint<PostCreateDto>
{
	public override void Configure()
	{
		Post("/posts/create");
		AllowFileUploads();
	}

	public override async Task HandleAsync(PostCreateDto req, CancellationToken ct)
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

		var result = await mediator.Send(new CreatePostCommand(req, fileDtos.ToList()), ct);

		await result.SendResponseAsync(HttpContext, ct);
	}
}