using SocialLink.SharedKernel;

namespace SocialLink.Notifications.Contracts.Details;

public class StartFollowingDetails : NotificationDetails
{
	public Guid FollowerId { get; set; }

	public string FollowerName { get; set; }

	public override eNotificationType NotificationTypeId => eNotificationType.StartFollowing;

	public override string Message
	{
		get => _message;

		set
		{
			if (string.IsNullOrWhiteSpace(value))
				_message = $"{FollowerName} has start following you.";
			else
				_message = value;
		}
	}
}