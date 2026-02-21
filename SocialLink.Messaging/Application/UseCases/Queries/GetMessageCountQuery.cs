using MongoDB.Driver;
using SocialLink.Common.Application;
using SocialLink.Messaging.Domain.Document;
using SocialLink.SharedKernel;

namespace SocialLink.Messaging.Application.UseCases.Queries;

internal sealed record GetMessageCountQuery : Query<long>, ICacheableQuery
{
	public string CacheKey => "messageCount";

	public TimeSpan? CacheDuration => TimeSpan.FromMinutes(15);
}

internal class GetMessageCountQueryHandler(IMongoMessagingDatabaseContext db) : MongoQueryHandler<GetMessageCountQuery, long>
{
	public override async Task<ResponseWrapper<long>> Handle(GetMessageCountQuery req, CancellationToken ct)
	{
		var filter = Builders<Message>.Filter.Ne(_ => _.Id, Guid.Empty);
		return new(await db.Messages.CountDocumentsAsync(filter, cancellationToken: ct));
	}
}
