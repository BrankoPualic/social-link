using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Commands;

namespace SocialLink.Users.Endpoints;

[Authorize]
internal class UpdateUser(IMediator mediator) : Endpoint<UserDto>
{
	public override void Configure()
	{
		Post("/users/update");
	}

	public override async Task HandleAsync(UserDto req, CancellationToken ct)
	{
		var result = await mediator.Send(new UpdateUserCommand(req), ct);

		await result.SendResponseAsync(HttpContext, ct);
	}
}