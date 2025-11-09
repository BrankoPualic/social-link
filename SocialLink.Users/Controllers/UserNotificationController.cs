using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialLink.SharedKernel.Domain;
using SocialLink.SharedKernel.Enumerators;
using SocialLink.SharedKernel.Extensions;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Commands;
using SocialLink.Users.Application.UseCases.Queries;

namespace SocialLink.Users.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
internal class UserNotificationController(IMediator mediator, IIdentityUser currentUser) : ControllerBase
{
	[HttpGet]
	public IActionResult GetList()
	{
		var list = Enum.GetValues<eNotificationType>()
			.Where(_ => _ is not eNotificationType.NotSet)
			.Select(_ => new NotificationPreferenceDto
			{
				UserId = currentUser.Id,
				NotificationTypeId = _,
				Name = _.GetDescription()
			})
			.ToList();

		return Ok(list);
	}

	[HttpGet("{userId}")]
	public async Task<IActionResult> GetUserPreferences([FromRoute] Guid userId, CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetNotificationPreferencesQuery(userId), ct);

		return Ok(result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> SavePreferences(List<NotificationPreferenceDto> request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new SaveNotificationPreferencesCommand(request), ct);

		return result.IsSuccess
			? NoContent()
			: BadRequest(result.Errors);
	}
}
