using SocialMedia.SharedKernel;

namespace SocialMedia.Notifications.Contracts.Details;

public abstract class NotificationDetails : INotificationDetails
{
	public Guid UserId { get; set; }

	public abstract eNotificationType NotificationTypeId { get; }

	public string Title { get; set; }

	public string Message { get; set; }
}