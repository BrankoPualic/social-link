using MongoDB.Driver;
using SocialLink.Notifications.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Notifications;

internal interface INotificationMongoContext : IMongoDatabaseContext
{
	IMongoCollection<Notification> Notifications { get; }
}