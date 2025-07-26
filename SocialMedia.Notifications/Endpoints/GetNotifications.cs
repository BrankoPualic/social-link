using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Notifications.Application;
using SocialMedia.Notifications.Application.Dtos;
using SocialMedia.Notifications.Application.UseCases.Queries;
using SocialMedia.SharedKernel;

namespace SocialMedia.Notifications.Endpoints;

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