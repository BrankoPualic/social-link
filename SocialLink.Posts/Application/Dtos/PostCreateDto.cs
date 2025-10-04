using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application.Dtos;

internal class PostCreateDto
{
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