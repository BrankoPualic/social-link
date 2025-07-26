using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Application.UseCases.Queries;

namespace SocialMedia.Users.Endpoints;

[Authorize]
internal class GetProfile(IMediator mediator) : Endpoint<Guid, UserDto>
{
	public override void Configure()
	{
		Get("/users/profile/{userId}");
	}

	public override async Task HandleAsync(Guid req, CancellationToken ct)
	{
		var result = await mediator.Send(new GetProfileQuery(req), ct);

		if (result.IsOk())
		{
			await Send.OkAsync(result, ct);
		}
		else if (result.IsNotFound())
		{
			await Send.NotFoundAsync(ct);
		}
	}
}