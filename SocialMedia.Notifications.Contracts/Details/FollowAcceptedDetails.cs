using SocialMedia.SharedKernel;

namespace SocialMedia.Notifications.Contracts.Details;

public class FollowAcceptedDetails : NotificationDetails
{
	public Guid FollowingId { get; set; }

	public string FollowingName { get; set; }

	public override eNotificationType NotificationTypeId => eNotificationType.FollowAccepted;

	public override string Message
	{
		get => _message;

		set
		{
			if (string.IsNullOrEmpty(value))
				_message = $"{FollowingName} has accepted your follow request.";
			else
				_message = value;
		}
	}
}