namespace SocialMedia.SharedKernel.Domain;

public interface IAuditedDomainModel
{
	Guid CreatedBy { get; set; }

	DateTime CreatedOn { get; set; }

	Guid LastChangedBy { get; set; }

	DateTime LastChangedOn { get; set; }
}