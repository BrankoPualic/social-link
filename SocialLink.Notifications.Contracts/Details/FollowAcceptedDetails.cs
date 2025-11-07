using SocialLink.SharedKernel.Enumerators;

namespace SocialLink.Notifications.Contracts.Details;

public class FollowAcceptedDetails : NotificationDetails
{
	public static string DefaultMessage(string followingName) => $"{followingName} has accepted your follow request.";

	public Guid FollowingId { get; set; }

	public string FollowingName { get; set; }

	public override eNotificationType NotificationTypeId => eNotificationType.FollowAccepted;
}