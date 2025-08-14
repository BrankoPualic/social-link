using SocialLink.SharedKernel;

namespace SocialLink.Notifications.Contracts.Details;

public class NotificationAction
{
	public string Label { get; set; }

	public string Endpoint { get; set; }

	public eNotificationActionMethodType? Method { get; set; }

	public bool RequiresConfirmation { get; set; }
}