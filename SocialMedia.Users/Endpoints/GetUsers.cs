using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.SharedKernel;
using SocialMedia.Users.Application;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Application.UseCases.Queries;

namespace SocialMedia.Users.Endpoints;

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