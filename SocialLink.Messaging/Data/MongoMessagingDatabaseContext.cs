using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SocialLink.Common.Data;
using SocialLink.Messaging.Domain.Document;
using SocialLink.SharedKernel.Domain;

namespace SocialLink.Messaging.Data;

internal sealed class MongoMessagingDatabaseContext : MongoDatabaseContext, IMongoMessagingDatabaseContext
{
	private readonly IMongoDatabase _db;

	public MongoMessagingDatabaseContext(IConfiguration config, IMongoClient client, IIdentityUser currentUser) : base(currentUser)
	{
		string databaseName = config.GetSection("MongoDatabases")["Messages"];
		_db = client.GetDatabase(databaseName);

		EnsureIndexes();
	}

	public IMongoCollection<Message> Messages => _db.GetCollection<Message>("Messages");

	// private

	private void EnsureIndexes()
	{
		var messageQueryIndex = Builders<Message>.IndexKeys
			.Ascending(_ => _.ChatGroupId)
			.Ascending(_ => _.CreatedOn);

		var userIdIndex = Builders<Message>.IndexKeys.Ascending(_ => _.UserId);

		var indexes = new List<CreateIndexModel<Message>>()
		{
			new(messageQueryIndex),
			new(userIdIndex),
		};

		Messages.Indexes.CreateMany(indexes);
	}
}