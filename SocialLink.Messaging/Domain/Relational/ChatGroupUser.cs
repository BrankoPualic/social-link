using System.ComponentModel.DataAnnotations.Schema;

namespace SocialLink.Messaging.Domain.Relational;

internal class ChatGroupUser
{
	public Guid ChatGroupId { get; set; }

	public Guid UserId { get; set; }

	public Guid LastReadMessageId { get; set; }

	public bool IsMuted { get; set; }

	public DateTime JoinedOn { get; set; }

	public DateTime LastChangedOn { get; set; }

	[ForeignKey(nameof(ChatGroupId))]
	public virtual ChatGroup ChatGroup { get; set; }
}