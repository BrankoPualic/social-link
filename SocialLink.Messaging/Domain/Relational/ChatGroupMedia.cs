using SocialLink.Messaging.Enumerators;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialLink.Messaging.Domain.Relational;

internal class ChatGroupMedia
{
	public Guid ChatGroupId { get; set; }

	public Guid BlobId { get; set; }

	public eChatGroupMedia? Type { get; set; }

	public bool IsActive { get; set; }

	public DateTime UploadedOn { get; set; }

	[ForeignKey(nameof(ChatGroupId))]
	public virtual ChatGroup ChatGroup { get; set; }
}