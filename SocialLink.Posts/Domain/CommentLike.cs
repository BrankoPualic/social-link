using System.ComponentModel.DataAnnotations.Schema;

namespace SocialLink.Posts.Domain;

internal class CommentLike
{
	public Guid UserId { get; set; }

	public Guid CommentId { get; set; }

	public DateTime LikedOn { get; set; }

	[ForeignKey(nameof(CommentId))]
	public virtual Comment Comment { get; set; }
}