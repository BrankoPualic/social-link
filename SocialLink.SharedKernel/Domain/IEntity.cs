namespace SocialLink.SharedKernel.Domain;

public interface IEntity<TKey> where TKey : struct
{
	TKey Id { get; set; }

	bool IsNew { get; }
}