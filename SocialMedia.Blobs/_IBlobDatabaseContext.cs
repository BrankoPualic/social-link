using MongoDB.Driver;
using SocialMedia.Blobs.Domain;
using SocialMedia.SharedKernel;

namespace SocialMedia.Blobs;

internal interface IBlobDatabaseContext : IMongoDatabaseContext
{
	IMongoCollection<Blob> Blobs { get; }
}