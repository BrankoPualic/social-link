using MongoDB.Driver;
using SocialLink.Common.Data;
using SocialLink.Notifications.Domain;

namespace SocialLink.Notifications;

internal interface INotificationMongoContext : IMongoDatabaseContext
{
	IMongoCollection<Notification> Notifications { get; }
}