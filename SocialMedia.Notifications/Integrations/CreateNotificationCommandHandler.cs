namespace SocialMedia.Notifications.Integrations;

//internal class CreateNotificationCommandHandler(INotificationMongoContext db) : CommandHandler<CreateNotificationCommand>(db)
//{
//	public override async Task<Result> Handle(CreateNotificationCommand req, CancellationToken ct)
//	{
//		var data = req.Data;

//		var model = new Notification
//		{
//			Id = Guid.NewGuid(),
//			UserId = data.UserId,
//			TypeId = data.NotificationTypeId,
//			Title = string.IsNullOrWhiteSpace(data.Title) ? data.NotificationTypeId.GetDescription() : data.Title,
//			Message = data.Message,
//			Details = data.SerializeJsonObject(),
//			CreatedOn = DateTime.UtcNow
//		};

//		db.Notifications.Add(model);

//		await db.SaveChangesAsync(false, ct);

//		return Result.Success();
//	}
//}