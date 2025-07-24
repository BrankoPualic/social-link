namespace SocialMedia.SharedKernel.Domain;

public interface IAuditedDomainModel
{
	Guid CreatedBy { get; }

	DateTime CreatedOn { get; }

	Guid LastChangedBy { get; }

	DateTime LastChangedOn { get; }
}
