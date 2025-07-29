using SocialMedia.SharedKernel.Domain;

namespace SocialMedia.Posts.Domain;

internal class Post : AuditedDomainModel<Guid>
{
	public Guid UserId { get; set; }

	public string Description { get; set; }

	public bool AllowComments { get; set; }

	public bool IsArchived { get; set; }

	public virtual ICollection<Comment> Comments { get; set; } = [];

	public virtual ICollection<PostMedia> Media { get; set; } = [];
}