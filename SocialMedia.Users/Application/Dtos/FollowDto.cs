using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Application.Dtos;

internal class FollowDto
{
	public Guid FollowerId { get; set; }

	public Guid FollowingId { get; set; }

	public void ToModel(UserFollow model)
	{
		model.FollowerId = FollowerId;
		model.FollowingId = FollowingId;
		model.FollowDate = DateTime.UtcNow;
	}
}