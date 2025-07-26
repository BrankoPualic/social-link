using SocialMedia.SharedKernel;

namespace SocialMedia.Notifications.Contracts.Details;

public interface INotificationDetails
{
	Guid UserId { get; }

	eNotificationType NotificationTypeId { get; }

	string Title { get; }

	string Message { get; }
}