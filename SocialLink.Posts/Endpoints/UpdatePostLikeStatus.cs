using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Application.UseCases.Commands;

namespace SocialLink.Posts.Endpoints;

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
