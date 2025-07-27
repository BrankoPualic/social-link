using Ardalis.Result;
using MongoDB.Driver;
using SocialMedia.Notifications.Application.Dtos;
using SocialMedia.Notifications.Domain;
using SocialMedia.SharedKernel;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Notifications.Application.UseCases.Queries;
internal sealed record GetNotificationsQuery(NotificationSearch Search) : Query<PagedResponse<NotificationDto>>;

internal class GetNotificationsQueryHandler(INotificationMongoContext db) : MongoQueryHandler<GetNotificationsQuery, PagedResponse<NotificationDto>>
{
	public override async Task<Result<PagedResponse<NotificationDto>>> Handle(GetNotificationsQuery req, CancellationToken ct)
	{
		var search = req.Search;

		var builder = Builders<Notification>.Filter;
		var filter = builder.Eq(_ => _.UserId, search.UserId) & builder.Eq(_ => _.IsRead, false);
		// NOTE: this could be if we add 3rd party service for mobile app notifications or something, or email notifications
		// & builder.Eq(_ => _.IsSent, true)

		var result = await db.Notifications.MongoSearchAsync(
			search,
			_ => _.CreatedOn,
			true,
			NotificationDto.Projection,
			filter,
			ct
		);

		return Result.Success(result);
	}
}