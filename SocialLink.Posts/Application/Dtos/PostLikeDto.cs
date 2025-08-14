namespace SocialLink.Posts.Application.Dtos;

internal class PostLikeDto
{
	public Guid PostId { get; set; }

	public Guid UserId { get; set; }
}
