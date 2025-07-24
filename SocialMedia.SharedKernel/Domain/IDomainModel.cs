namespace SocialMedia.SharedKernel.Domain;

public interface IDomainModel<TKey> : IEntity<TKey> where TKey : struct
{
}
