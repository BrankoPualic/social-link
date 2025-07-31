using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.Posts.Application.UseCases.Commands;

namespace SocialMedia.Posts.Endpoints;

[Authorize]
internal class UpdatePostLikeStatus(IMediator mediator) : Endpoint<PostLikeDto>
{
	public override void Configure()
	{
		Post("/posts/like");
	}

	public override async Task HandleAsync(PostLikeDto req, CancellationToken ct)
	{
		await mediator.Send(new UpdatePostLikeStatusCommand(req), ct);
		await Send.NoContentAsync(ct);
	}
}
