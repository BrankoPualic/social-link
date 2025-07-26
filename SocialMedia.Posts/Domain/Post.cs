using SocialMedia.SharedKernel.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Posts.Domain;

internal class Post : AuditedDomainModel<Guid>
{
	public Guid UserId { get; set; }

	public string Description { get; set; }

	public bool AllowComments { get; set; }

	public bool IsArchived { get; set; }

	[InverseProperty(nameof(Comment.Post))]
	public virtual ICollection<Comment> Comments { get; set; } = [];
}