using SocialMedia.SharedKernel;
using SocialMedia.SharedKernel.Domain;

namespace SocialMedia.Blobs.Domain;

internal class Blob : AuditedDomainModel<Guid>
{
	public eBlobType TypeId { get; set; }

	public string Url { get; set; }

	public string Name { get; set; }

	public string Type { get; set; }

	public long? Size { get; set; }

	public bool IsActive { get; set; }
}