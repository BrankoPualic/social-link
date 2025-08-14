using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Users.Application;
using SocialLink.Users.Application.Dtos;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.UseCases.Queries;

namespace SocialLink.Users.Endpoints;

[Authorize]
internal class GetUsers(IMediator mediator) : Endpoint<UserSearch, PagedResponse<UserDto>>
{
	public override void Configure()
	{
		Post("/users");
	}

	public override async Task HandleAsync(UserSearch req, CancellationToken ct)
	{
		var result = await mediator.Send(new GetUsersQuery(req), ct);
		await Send.OkAsync(result, ct);
	}
}