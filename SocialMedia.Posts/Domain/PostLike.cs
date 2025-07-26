using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Posts.Domain;

internal class PostLike
{
	public Guid UserId { get; set; }

	public Guid PostId { get; set; }

	public DateTime LikedOn { get; set; }

	[ForeignKey(nameof(PostId))]
	public virtual Post Post { get; set; }
}
