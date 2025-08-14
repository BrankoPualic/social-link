using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Application.UseCases.Commands;

namespace SocialLink.Posts.Endpoints;

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