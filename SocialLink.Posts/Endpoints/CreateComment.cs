using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Application.UseCases.Commands;

namespace SocialLink.Posts.Endpoints;

[Authorize]
internal class CreateComment(IMediator mediator) : Endpoint<CommentEditDto>
{
	public override void Configure()
	{
		Post("/posts/comments/create");
	}

	public override async Task HandleAsync(CommentEditDto req, CancellationToken ct)
	{
		var result = await mediator.Send(new CreateCommentCommand(req), ct);

		if (result.IsCreated())
		{
			await Send.OkAsync(result, ct);
		}
		else if (result.IsInvalid())
		{
			await Send.ErrorsAsync(400, ct);
		}
	}
}