using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Notifications.Application.Dtos;
using SocialLink.Notifications.Application.UseCases.Commands;

namespace SocialLink.Notifications.Endpoints;

[Authorize]
internal class ReadNotification(IMediator mediator) : Endpoint<NotificationDto>
{
	public override void Configure()
	{
		Post("/notifications/read");
	}

	public override async Task HandleAsync(NotificationDto req, CancellationToken ct)
	{
		var result = await mediator.Send(new ReadNotificationCommand(req), ct);

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