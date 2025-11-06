using MongoDB.Driver;
using SocialLink.Notifications.Application.Dtos;
using SocialLink.Notifications.Domain;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Notifications.Application.UseCases.Queries;
internal sealed record GetNotificationsQuery(NotificationSearch Search) : Query<PagedResponse<NotificationDto>>;

internal class GetNotificationsQueryHandler(INotificationMongoContext db) : MongoQueryHandler<GetNotificationsQuery, PagedResponse<NotificationDto>>
{
	public override async Task<ResponseWrapper<PagedResponse<NotificationDto>>> Handle(GetNotificationsQuery req, CancellationToken ct)
	{
		var search = req.Search;

		var builder = Builders<Notification>.Filter;
		var filter = builder.Eq(_ => _.UserId, search.UserId) & builder.Eq(_ => _.IsRead, false);

		var result = await db.Notifications.MongoSearchAsync(
			search,
			_ => _.CreatedOn,
			true,
			NotificationDto.Projection,
			filter,
			ct
		);

		return new(result);
	}
}