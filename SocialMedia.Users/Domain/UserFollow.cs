using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Users.Domain;

internal class UserFollow
{
	public Guid FollowerId { get; set; }

	public Guid FollowingId { get; set; }

	public DateTime FollowDate { get; set; }

	[ForeignKey(nameof(FollowerId))]
	public virtual User Follower { get; set; }

	[ForeignKey(nameof(FollowingId))]
	public virtual User Following { get; set; }
}