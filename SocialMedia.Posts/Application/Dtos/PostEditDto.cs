using SocialMedia.Posts.Domain;
using SocialMedia.SharedKernel;

namespace SocialMedia.Posts.Application.Dtos;

internal class PostEditDto
{
	public Guid? Id { get; set; }

	public Guid UserId { get; set; }

	public string Description { get; set; }

	public bool AllowComments { get; set; }

	public void ToModel(Post model)
	{
		model.GenerateIdIfNew();

		model.UserId = UserId;
		model.Description = Description;
		model.AllowComments = AllowComments;
	}
}