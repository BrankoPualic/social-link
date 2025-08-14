using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Queries;

namespace SocialLink.Users.Endpoints;

[Authorize]
internal class GetProfile(IMediator mediator) : Endpoint<GetProfileRequest, UserDto>
{
	public override void Configure()
	{
		Get("/users/profile/{UserId}");
	}

	public override async Task HandleAsync(GetProfileRequest req, CancellationToken ct)
	{
		var result = await mediator.Send(new GetProfileQuery(req.UserId), ct);

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