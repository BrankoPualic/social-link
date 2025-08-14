using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Queries;

namespace SocialLink.Users.Endpoints;

[Authorize]
internal class CheckFollowStatus(IMediator mediator) : Endpoint<FollowDto, bool>
{
	public override void Configure()
	{
		Post("/users/checkFollowStatus");
	}

	public override async Task HandleAsync(FollowDto req, CancellationToken ct)
	{
		var result = await mediator.Send(new CheckFollowStatusQuery(req), ct);
		await Send.OkAsync(result, ct);
	}
}