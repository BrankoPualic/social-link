using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Application.Dtos;

namespace SocialMedia.Users.Application.UseCases.Queries;

internal sealed record GetNotificationPreferencesQuery(Guid UserId) : Query<List<NotificationPreferenceDto>>;

internal class GetNotificationPreferencesQueryHandler(IUserDatabaseContext db) : EFQueryHandler<GetNotificationPreferencesQuery, List<NotificationPreferenceDto>>(db)
{
	public override async Task<Result<List<NotificationPreferenceDto>>> Handle(GetNotificationPreferencesQuery req, CancellationToken ct)
	{
		var userId = req.UserId;

		var result = (await db.NotificationPreferences
			.Where(_ => _.UserId == userId)
			.ToListAsync(ct))
			.Select(NotificationPreferenceDto.InMemoryProjection)
			.ToList();

		return Result.Success(result);
	}
}