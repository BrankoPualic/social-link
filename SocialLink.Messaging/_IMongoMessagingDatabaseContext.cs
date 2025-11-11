using MongoDB.Driver;
using SocialLink.Common.Data;
using SocialLink.Messaging.Domain.Document;

namespace SocialLink.Messaging;

internal interface IMongoMessagingDatabaseContext : IMongoDatabaseContext
{
	IMongoCollection<Message> Messages { get; }
}