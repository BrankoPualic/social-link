using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialLink.Notifications.Application;
using SocialLink.Notifications.Application.Dtos;
using SocialLink.Notifications.Application.UseCases.Commands;
using SocialLink.Notifications.Application.UseCases.Queries;

namespace SocialLink.Notifications.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
internal class NotificationController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> Get(NotificationSearch request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetNotificationsQuery(request), ct);
		return Ok(result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Read(NotificationDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new ReadNotificationCommand(request), ct);

		return result.IsSuccess
			? NoContent()
			: BadRequest(result.Errors);
	}
}
