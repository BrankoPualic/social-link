using SocialLink.SharedKernel.Domain;

namespace SocialLink.Messaging.Domain.Relational;

internal class ChatGroup : AuditedDomainModel<Guid>
{
	public DateTime? LastMessageOn { get; set; }

	public string LastMessagePreview { get; set; }

	public bool? IsGroup { get; set; }

	public string Name { get; set; }

	public virtual ICollection<ChatGroupUser> Users { get; set; } = [];
}