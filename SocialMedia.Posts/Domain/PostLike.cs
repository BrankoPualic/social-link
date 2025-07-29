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

internal class PostMedia
{
	public Guid PostId { get; set; }

	public Guid BlobId { get; set; }

	public bool IsActive { get; set; }

	public DateTime UploadedOn { get; set; }

	[ForeignKey(nameof(PostId))]
	public virtual Post Post { get; set; }
}