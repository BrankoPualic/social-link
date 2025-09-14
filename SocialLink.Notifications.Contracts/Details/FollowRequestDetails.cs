using SocialLink.SharedKernel;

namespace SocialLink.Notifications.Contracts.Details;

public class FollowRequestDetails : NotificationDetails
{
	public static string DefaultMessage(string followerName) => $"{followerName} has requested to follow you.";

	public Guid FollowerId { get; set; }

	public string FollowerName { get; set; }

	public override eNotificationType NotificationTypeId => eNotificationType.FollowRequest;
}