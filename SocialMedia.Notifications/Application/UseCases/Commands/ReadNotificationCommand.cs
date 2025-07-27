using Ardalis.Result;
using MongoDB.Driver;
using SocialMedia.Notifications.Application.Dtos;
using SocialMedia.Notifications.Domain;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Notifications.Application.UseCases.Commands;
internal sealed record ReadNotificationCommand(NotificationDto Data) : Command;

internal class ReadNotificationCommandHandler(INotificationMongoContext db) : MongoCommandHandler<ReadNotificationCommand>
{
	public override async Task<Result> Handle(ReadNotificationCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var filterBuilder = Builders<Notification>.Filter;
		var filter = filterBuilder.Eq(_ => _.Id, data.Id);

		var updateBuilder = Builders<Notification>.Update;
		var update = updateBuilder.Set(_ => _.IsRead, true);

		var model = await db.Notifications.FindOneAndUpdateAsync(filter, update, cancellationToken: ct);
		if (model is null)
			return Result.NotFound("Notification not found.");

		return Result.NoContent();
	}
}