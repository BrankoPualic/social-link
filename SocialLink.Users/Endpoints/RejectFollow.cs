using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Commands;

namespace SocialLink.Users.Endpoints;

[Authorize]
internal class RejectFollow(IMediator mediator) : Endpoint<FollowDto>
{
	public override void Configure()
	{
		Post("/users/rejectFollow");
	}

	public override async Task HandleAsync(FollowDto req, CancellationToken ct)
	{
		var result = await mediator.Send(new RejectFollowCommand(req), ct);

		await result.SendResponseAsync(HttpContext, ct);
	}
}