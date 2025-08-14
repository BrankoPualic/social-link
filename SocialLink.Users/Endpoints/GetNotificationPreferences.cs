using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Queries;

namespace SocialLink.Users.Endpoints;

[Authorize]
internal class GetNotificationPreferences(IMediator mediator) : Endpoint<GetNotificationPreferencesRequest, List<NotificationPreferenceDto>>
{
	public override void Configure()
	{
		Get("/users/notificationPreferences/{UserId}");
	}

	public override async Task HandleAsync(GetNotificationPreferencesRequest req, CancellationToken ct)
	{
		var result = await mediator.Send(new GetNotificationPreferencesQuery(req.UserId), ct);
		await Send.OkAsync(result, ct);
	}
}