using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Application.UseCases.Queries;

namespace SocialMedia.Users.Endpoints;

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