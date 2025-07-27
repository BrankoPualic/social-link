using MongoDB.Driver;
using SocialMedia.Notifications.Domain;

namespace SocialMedia.Notifications;

internal interface INotificationMongoContext
{
	IMongoCollection<Notification> Notifications { get; }
}