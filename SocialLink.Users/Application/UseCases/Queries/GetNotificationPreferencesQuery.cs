using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Enumerators;
using SocialLink.SharedKernel.Extensions;
using SocialLink.Users.Application.Dtos;

namespace SocialLink.Users.Application.UseCases.Queries;

internal sealed record GetNotificationPreferencesQuery(Guid UserId) : Query<List<NotificationPreferenceDto>>;

internal class GetNotificationPreferencesQueryHandler(IUserDatabaseContext db) : EFQueryHandler<GetNotificationPreferencesQuery, List<NotificationPreferenceDto>>(db)
{
	public override async Task<ResponseWrapper<List<NotificationPreferenceDto>>> Handle(GetNotificationPreferencesQuery req, CancellationToken ct)
	{
		var userId = req.UserId;

		var existingPreferences = (await db.NotificationPreferences
			.Where(_ => _.UserId == userId)
			.ToListAsync(ct))
			.Select(NotificationPreferenceDto.InMemoryProjection)
			.ToList();

		var allPreferences = Enum.GetValues<eNotificationType>()
			.Where(_ => _ is not eNotificationType.NotSet)
			.Select(e =>
			{
				var overridden = existingPreferences.FirstOrDefault(_ => _.NotificationTypeId == e);
				return new NotificationPreferenceDto
				{
					Id = overridden?.Id ?? Guid.Empty,
					UserId = overridden?.UserId ?? userId,
					NotificationTypeId = e,
					Name = e.GetDescription(),
					IsMuted = overridden?.IsMuted ?? false,
				};
			})
			.ToList();

		return new(allPreferences);
	}
}