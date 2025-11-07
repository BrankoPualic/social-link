using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SocialLink.Common.Data;
using SocialLink.Notifications.Domain;
using SocialLink.SharedKernel.Domain;

namespace SocialLink.Notifications.Data;

internal sealed class NotificationMongoContext : MongoDatabaseContext, INotificationMongoContext
{
	private readonly IMongoDatabase _db;

	public NotificationMongoContext(IConfiguration config, IMongoClient client, IIdentityUser currentUser) : base(currentUser)
	{
		string databaseName = config.GetSection("MongoDatabases")["Notifications"];
		_db = client.GetDatabase(databaseName);

		EnsureIndexes();
	}

	public IMongoCollection<Notification> Notifications => _db.GetCollection<Notification>("Notifications");

	// private

	private void EnsureIndexes()
	{
		var createdOnIndex = Builders<Notification>.IndexKeys.Ascending(_ => _.CreatedOn);
		var isReadIndex = Builders<Notification>.IndexKeys.Ascending(_ => _.IsRead);
		var userQueryIndex = Builders<Notification>.IndexKeys
			.Ascending(_ => _.UserId)
			.Ascending(_ => _.IsRead);

		var indexes = new List<CreateIndexModel<Notification>>(){
			new(createdOnIndex),
			new(isReadIndex),
			new(userQueryIndex),
		};

		Notifications.Indexes.CreateMany(indexes);
	}
}