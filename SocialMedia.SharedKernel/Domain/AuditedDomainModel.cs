namespace SocialMedia.SharedKernel.Domain;

public class AuditedDomainModel<TKey> : DomainModel<TKey>, IAuditedDomainModel
	where TKey : struct
{
	public Guid CreatedBy { get; set; }

	public DateTime CreatedOn { get; set; }

	public Guid LastChangedBy { get; set; }

	public DateTime LastChangedOn { get; set; }
}