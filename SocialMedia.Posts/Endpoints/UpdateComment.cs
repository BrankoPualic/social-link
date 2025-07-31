using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.Posts.Application.UseCases.Commands;

namespace SocialMedia.Posts.Endpoints;

[Authorize]
internal class UpdateComment(IMediator mediator) : Endpoint<CommentEditDto>
{
	public override void Configure()
	{
		Post("/posts/comments/update");
	}

	public override async Task HandleAsync(CommentEditDto req, CancellationToken ct)
	{
		var result = await mediator.Send(new UpdateCommentCommand(req), ct);

		if (result.IsNoContent())
		{
			await Send.NoContentAsync(ct);
		}
		else if (result.IsNotFound())
		{
			await Send.NotFoundAsync(ct);
		}
	}
}