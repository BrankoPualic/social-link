using Ardalis.Result;
using SocialMedia.Notifications.Contracts;
using SocialMedia.Notifications.Domain;
using SocialMedia.SharedKernel;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Notifications.Integrations;

internal class CreateNotificationCommandHandler(INotificationMongoContext db) : MongoCommandHandler<CreateNotificationCommand>
{
	public override async Task<Result> Handle(CreateNotificationCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = new Notification
		{
			Id = Guid.NewGuid(),
			UserId = data.UserId,
			TypeId = data.NotificationTypeId,
			Title = string.IsNullOrWhiteSpace(data.Title) ? data.NotificationTypeId.GetDescription() : data.Title,
			Message = data.Message,
			Details = data.SerializeJsonObject(),
			CreatedOn = DateTime.UtcNow
		};

		await db.Notifications.InsertOneAsync(model, cancellationToken: ct);

		return Result.Success();
	}
}