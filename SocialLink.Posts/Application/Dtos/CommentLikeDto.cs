namespace SocialLink.Posts.Application.Dtos;

internal class CommentLikeDto
{
	public Guid CommentId { get; set; }

	public Guid UserId { get; set; }
}