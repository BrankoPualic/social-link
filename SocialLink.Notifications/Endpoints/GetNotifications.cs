using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Notifications.Application;
using SocialLink.Notifications.Application.Dtos;
using SocialLink.Notifications.Application.UseCases.Queries;
using SocialLink.SharedKernel;

namespace SocialLink.Notifications.Endpoints;

[Authorize]
internal class GetNotifications(IMediator mediator) : Endpoint<NotificationSearch, PagedResponse<NotificationDto>>
{
	public override void Configure()
	{
		Post("/notifications");
	}

	public override async Task HandleAsync(NotificationSearch req, CancellationToken ct)
	{
		var result = await mediator.Send(new GetNotificationsQuery(req), ct);
		await Send.OkAsync(result, ct);
	}
}