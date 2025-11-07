using SocialLink.SharedKernel.Enumerators;

namespace SocialLink.Notifications.Contracts.Details;

public interface INotificationDetails
{
	Guid UserId { get; }

	eNotificationType NotificationTypeId { get; }

	string Title { get; }

	string Message { get; }
}