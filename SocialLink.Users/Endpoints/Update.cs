using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Commands;

namespace SocialLink.Users.Endpoints;

[Authorize]
internal class Update(IMediator mediator) : Endpoint<UserDto>
{
	public override void Configure()
	{
		Post("/users/update");
	}

	public override async Task HandleAsync(UserDto req, CancellationToken ct)
	{
		var result = await mediator.Send(new UpdateCommand(req), ct);

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