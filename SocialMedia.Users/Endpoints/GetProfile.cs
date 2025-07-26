using Ardalis.Result;
using FastEndpoints;
using MediatR;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Application.UseCases.Queries;

namespace SocialMedia.Users.Endpoints;

//[Authorize]
internal class GetProfile(IMediator mediator) : Endpoint<GetProfileRequest, UserDto>
{
	public override void Configure()
	{
		Get("/users/profile/{UserId}");
		AllowAnonymous();
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