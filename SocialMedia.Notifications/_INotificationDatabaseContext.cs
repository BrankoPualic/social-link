using MongoDB.Driver;
using SocialMedia.Notifications.Domain;
using SocialMedia.SharedKernel;

namespace SocialMedia.Notifications;

internal interface INotificationMongoContext : IMongoDatabaseContext
{
	IMongoCollection<Notification> Notifications { get; }
}