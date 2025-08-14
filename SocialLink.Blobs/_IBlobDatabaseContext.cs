using MongoDB.Driver;
using SocialLink.Blobs.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Blobs;

internal interface IBlobDatabaseContext : IMongoDatabaseContext
{
	IMongoCollection<Blob> Blobs { get; }
}