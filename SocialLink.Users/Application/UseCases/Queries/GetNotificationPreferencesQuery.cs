using Microsoft.EntityFrameworkCore;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Application.Dtos;

namespace SocialLink.Users.Application.UseCases.Queries;

internal sealed record GetNotificationPreferencesQuery(Guid UserId) : Query<List<NotificationPreferenceDto>>;

internal class GetNotificationPreferencesQueryHandler(IUserDatabaseContext db) : EFQueryHandler<GetNotificationPreferencesQuery, List<NotificationPreferenceDto>>(db)
{
	public override async Task<ResponseWrapper<List<NotificationPreferenceDto>>> Handle(GetNotificationPreferencesQuery req, CancellationToken ct)
	{
		var userId = req.UserId;

		var result = (await db.NotificationPreferences
			.Where(_ => _.UserId == userId)
			.ToListAsync(ct))
			.Select(NotificationPreferenceDto.InMemoryProjection)
			.ToList();

		return new(result);
	}
}