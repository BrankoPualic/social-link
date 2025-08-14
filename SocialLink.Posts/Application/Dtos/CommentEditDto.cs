using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application.Dtos;

internal class CommentEditDto
{
	public Guid? Id { get; set; }

	public Guid UserId { get; set; }

	public Guid PostId { get; set; }

	public Guid? ParentId { get; set; }

	public string Message { get; set; }

	public void ToModel(Comment model)
	{
		model.GenerateIdIfNew();

		model.UserId = UserId;
		model.PostId = PostId;
		model.ParentId = ParentId;
		model.Message = Message;
	}
}