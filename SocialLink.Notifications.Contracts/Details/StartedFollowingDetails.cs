using SocialLink.SharedKernel;

namespace SocialLink.Notifications.Contracts.Details;

public class StartFollowingDetails : NotificationDetails
{
	public static string DefaultMessage(string followerName) => $"{followerName} has started following you.";

	public Guid FollowerId { get; set; }

	public string FollowerName { get; set; }

	public override eNotificationType NotificationTypeId => eNotificationType.StartFollowing;
}