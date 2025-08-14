using SocialLink.SharedKernel;

namespace SocialLink.Notifications.Contracts.Details;

public class FollowRequestDetails : NotificationDetails
{
	public Guid FollowerId { get; set; }

	public string FollowerName { get; set; }

	public override eNotificationType NotificationTypeId => eNotificationType.FollowRequest;

	public override string Message
	{
		get => _message;

		set
		{
			if (string.IsNullOrWhiteSpace(value))
				_message = $"{FollowerName} has requested to follow you.";
			else
				_message = value;
		}
	}
}