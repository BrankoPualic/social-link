using SocialLink.SharedKernel;

namespace SocialLink.Notifications.Contracts.Details;

public abstract class NotificationDetails : INotificationDetails
{
	protected string _message;

	public Guid UserId { get; set; }

	public abstract eNotificationType NotificationTypeId { get; }

	public string Title { get; set; }

	public virtual string Message { get; set; }

	public List<NotificationAction> Actions { get; set; } = [];
}