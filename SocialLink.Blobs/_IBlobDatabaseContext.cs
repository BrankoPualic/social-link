using MongoDB.Driver;
using SocialLink.Blobs.Domain;
using SocialLink.Common.Data;

namespace SocialLink.Blobs;

internal interface IBlobDatabaseContext : IMongoDatabaseContext
{
	IMongoCollection<Blob> Blobs { get; }
}