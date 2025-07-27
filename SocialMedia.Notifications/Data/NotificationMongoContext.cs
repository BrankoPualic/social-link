using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SocialMedia.Notifications.Domain;

namespace SocialMedia.Notifications.Data;

internal sealed class NotificationMongoContext : INotificationMongoContext
{
	private readonly IMongoDatabase _db;

	public NotificationMongoContext(IConfiguration config, IMongoClient client)
	{
		string databaseName = config.GetSection("MongoDatabases")["Notifications"];
		_db = client.GetDatabase(databaseName);
	}

	public IMongoCollection<Notification> Notifications => _db.GetCollection<Notification>("Notifications");
}