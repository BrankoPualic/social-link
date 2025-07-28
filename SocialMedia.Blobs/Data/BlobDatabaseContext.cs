using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SocialMedia.Blobs.Domain;

namespace SocialMedia.Blobs.Data;

internal sealed class BlobDatabaseContext : IBlobDatabaseContext
{
	private readonly IMongoDatabase _db;

	public BlobDatabaseContext(IConfiguration config, IMongoClient client)
	{
		string databaseName = config.GetSection("MongoDatabases")["Blobs"];
		_db = client.GetDatabase(databaseName);

		EnsureIndexes();
	}

	public IMongoCollection<Blob> Blobs => _db.GetCollection<Blob>("Blobs");

	// private

	private void EnsureIndexes()
	{
		var typeIdIndex = Builders<Blob>.IndexKeys.Ascending(_ => _.TypeId);
		var userQueryIndex = Builders<Blob>.IndexKeys
			.Ascending(_ => _.TypeId)
			.Ascending(_ => _.IsActive);

		var indexes = new List<CreateIndexModel<Blob>>(){
			new(typeIdIndex),
			new(userQueryIndex),
		};

		Blobs.Indexes.CreateMany(indexes);
	}
}