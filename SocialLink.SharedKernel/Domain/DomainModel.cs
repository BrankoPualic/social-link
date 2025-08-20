using MongoDB.Bson.Serialization.Attributes;

namespace SocialLink.SharedKernel.Domain;

public class DomainModel<TKey> : IDomainModel<TKey> where TKey : struct
{
	public TKey Id { get; set; }

	public bool IsNew => EqualityComparer<TKey>.Default.Equals(Id, default);
}

public class MongoDomainModel<TKey> : IDomainModel<TKey> where TKey : struct
{
	[BsonId]
	public TKey Id { get; set; }

	public bool IsNew => EqualityComparer<TKey>.Default.Equals(Id, default);
}