using MongoDB.Driver;
using SocialLink.SharedKernel.Domain;

namespace SocialLink.Common.Data;

public static class MongoAuditExtensions
{
	public static UpdateDefinition<T> WithAudit<T>(this UpdateDefinition<T> update, Guid userId, bool isNew = false) where T : IAuditedDomainModel
	{
		var now = DateTime.UtcNow;

		var builder = Builders<T>.Update.Combine(
			update,
			Builders<T>.Update
				.Set(_ => _.LastChangedOn, now)
				.Set(_ => _.LastChangedBy, userId)
		);

		if (isNew)
		{
			builder = Builders<T>.Update.Combine(
				builder,
				Builders<T>.Update
					.Set(_ => _.CreatedOn, now)
					.Set(_ => _.CreatedBy, userId)
			);
		}

		return builder;
	}
}