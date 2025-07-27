using MongoDB.Bson.Serialization.Attributes;

namespace SocialMedia.SharedKernel.Domain;

public class DomainModel<TKey> : IDomainModel<TKey> where TKey : struct
{
	public TKey Id { get; set; }

	public bool IsNew => Id.Equals(default);
}

public class MongoDomainModel<TKey> : IDomainModel<TKey> where TKey : struct
{
	[BsonId]
	public TKey Id { get; set; }

	public bool IsNew => Id.Equals(default);
}