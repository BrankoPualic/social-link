using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Users.Domain;

internal class UserMedia
{
	public Guid UserId { get; set; }

	public Guid BlobId { get; set; }

	public bool IsActive { get; set; }

	public DateTime UploadedOn { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User User { get; set; }
}