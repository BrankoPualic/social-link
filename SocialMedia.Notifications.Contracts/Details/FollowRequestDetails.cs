using SocialMedia.SharedKernel;

namespace SocialMedia.Notifications.Contracts.Details;

public class FollowRequestDetails : NotificationDetails
{
	public Guid FollowerId { get; set; }

	public string FollowerName { get; set; }

	public override eNotificationType NotificationTypeId => eNotificationType.FollowRequest;
}