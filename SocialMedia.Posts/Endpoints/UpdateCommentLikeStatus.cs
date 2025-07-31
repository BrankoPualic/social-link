using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.Posts.Application.UseCases.Commands;

namespace SocialMedia.Posts.Endpoints;

[Authorize]
internal class UpdateCommentLikeStatus(IMediator mediator) : Endpoint<CommentLikeDto>
{
	public override void Configure()
	{
		Post("/posts/comments/like");
	}

	public override async Task HandleAsync(CommentLikeDto req, CancellationToken ct)
	{
		await mediator.Send(new UpdateCommentLikeStatusCommand(req), ct);
		await Send.NoContentAsync(ct);
	}
}