using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Users.Application.Dtos;
using SocialLink.SharedKernel;

namespace SocialLink.Users.Endpoints;

[Authorize]
internal class GetAllPossibleNotificationPreferences(IIdentityUser currentUser) : Endpoint<List<NotificationPreferenceDto>>
{
	public override void Configure()
	{
		Get("/users/possibleNotificationPreferences");
	}

	public override Task HandleAsync(List<NotificationPreferenceDto> req, CancellationToken ct)
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

		return Send.OkAsync(list, ct);
	}
}