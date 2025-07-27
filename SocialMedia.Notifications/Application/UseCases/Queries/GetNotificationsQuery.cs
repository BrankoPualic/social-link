using SocialMedia.Notifications.Application.Dtos;
using SocialMedia.SharedKernel;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Notifications.Application.UseCases.Queries;
internal sealed record GetNotificationsQuery(NotificationSearch Search) : Query<PagedResponse<NotificationDto>>;

//internal class GetNotificationsQueryHandler(INotificationMongoContext db) : QueryHandler<GetNotificationsQuery, PagedResponse<NotificationDto>>(db)
//{
//	public override async Task<Result<PagedResponse<NotificationDto>>> Handle(GetNotificationsQuery req, CancellationToken ct)
//	{
//		//var search = req.Search;

//		//var filters = new List<Expression<Func<Notification, bool>>>
//		//{
//		//	_ => !_.IsRead,
//		//	// _ => _ => _.IsSent NOTE: this could be if we add 3rd party service for mobile app notifications or something, or email notifications
//		//	_ => _.UserId == search.UserId
//		//};

//		//var result = await db.Notifications.SearchAsync(
//		//	search,
//		//	_ => _.CreatedOn,
//		//	true,
//		//	NotificationDto.Projection,
//		//	filters,
//		//	ct
//		//);

//		var result = new PagedResponse<NotificationDto>();

//		return Result.Success(result);
//	}
//}