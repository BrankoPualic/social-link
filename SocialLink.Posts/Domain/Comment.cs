using SocialLink.SharedKernel.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialLink.Posts.Domain;

internal class Comment : AuditedDomainModel<Guid>
{
	public Guid UserId { get; set; }

	public Guid PostId { get; set; }

	public Guid? ParentId { get; set; }

	public string Message { get; set; }

	[ForeignKey(nameof(PostId))]
	public virtual Post Post { get; set; }

	[ForeignKey(nameof(ParentId))]
	public virtual Comment Parent { get; set; }

	[InverseProperty(nameof(Parent))]
	public virtual ICollection<Comment> Replies { get; set; } = [];

	public virtual ICollection<CommentLike> Likes { get; set; } = [];
}