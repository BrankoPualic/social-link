namespace SocialMedia.SharedKernel.Domain;

public class DomainModel<TKey> : IDomainModel<TKey> where TKey : struct
{
	public TKey Id { get; private set; }

	public bool IsNew => Id.Equals(default);
}