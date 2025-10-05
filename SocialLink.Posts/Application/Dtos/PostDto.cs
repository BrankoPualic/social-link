using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.Users.Contracts;
using System.Linq.Expressions;

namespace SocialLink.Posts.Application.Dtos;

internal class PostDto
{
	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public string Description { get; set; }

	public bool AllowComments { get; set; }

	public long? LikesCount { get; set; }

	public long? CommentsCount { get; set; }

	public DateTime CreatedOn { get; set; }

	public UserContractDto User { get; set; }

	public List<BlobDto> Media { get; set; } = [];

	public bool? IsLiked { get; set; }

	public static Expression<Func<Post, PostDto>> Projection => _ => new()
	{
		Id = _.Id,
		UserId = _.UserId,
		Description = _.Description,
		AllowComments = _.AllowComments,
		CreatedOn = _.CreatedOn,
	};
}