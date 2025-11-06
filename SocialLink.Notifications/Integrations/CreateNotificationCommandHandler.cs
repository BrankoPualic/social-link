using SocialLink.Notifications.Contracts;
using SocialLink.Notifications.Domain;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Notifications.Integrations;

internal class CreateNotificationCommandHandler(INotificationMongoContext db) : MongoCommandHandler<CreateNotificationCommand>
{
	public override async Task<ResponseWrapper> Handle(CreateNotificationCommand req, CancellationToken ct)
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

		return new();
	}
}