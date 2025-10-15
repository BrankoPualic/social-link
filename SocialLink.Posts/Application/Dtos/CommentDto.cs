using SocialLink.Posts.Domain;
using SocialLink.Users.Contracts;
using System.Linq.Expressions;

namespace SocialLink.Posts.Application.Dtos;

internal class CommentDto
{
	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public Guid PostId { get; set; }

	public Guid? ParentId { get; set; }

	public string Message { get; set; }

	public long? LikesCount { get; set; }

	public long? RepliesCount { get; set; }

	public DateTime CreatedOn { get; set; }

	public UserContractDto User { get; set; }

	public bool? IsLiked { get; set; }

	public static Expression<Func<Comment, CommentDto>> Projection => _ => new()
	{
		Id = _.Id,
		UserId = _.UserId,
		PostId = _.PostId,
		ParentId = _.ParentId,
		Message = _.Message,
		CreatedOn = _.CreatedOn,
	};
}