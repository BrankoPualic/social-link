using System.ComponentModel.DataAnnotations.Schema;

namespace SocialLink.Posts.Domain;

internal class PostMedia
{
	public Guid PostId { get; set; }

	public Guid BlobId { get; set; }

	public ePostMedia? Type { get; set; }

	public int Order { get; set; }

	public bool IsActive { get; set; }

	public DateTime UploadedOn { get; set; }

	[ForeignKey(nameof(PostId))]
	public virtual Post Post { get; set; }
}