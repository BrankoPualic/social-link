using MongoDB.Bson.Serialization.Attributes;

namespace SocialMedia.SharedKernel.Domain;

public class AuditedDomainModel<TKey> : DomainModel<TKey>, IAuditedDomainModel
	where TKey : struct
{
	public Guid CreatedBy { get; set; }

	public DateTime CreatedOn { get; set; }

	public Guid LastChangedBy { get; set; }

	public DateTime LastChangedOn { get; set; }
}

public class MongoAuditedDomainModel<TKey> : MongoDomainModel<TKey>, IAuditedDomainModel
	where TKey : struct
{
	public Guid CreatedBy { get; set; }

	[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
	public DateTime CreatedOn { get; set; }

	public Guid LastChangedBy { get; set; }

	[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
	public DateTime LastChangedOn { get; set; }
}