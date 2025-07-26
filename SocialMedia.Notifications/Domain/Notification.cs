using SocialMedia.SharedKernel;
using SocialMedia.SharedKernel.Domain;

namespace SocialMedia.Notifications.Domain;

internal class Notification : DomainModel<Guid>
{
	public Guid UserId { get; set; }

	public eNotificationType TypeId { get; set; }

	public string Title { get; set; }

	public string Message { get; set; }

	public string Details { get; set; }

	public bool IsRead { get; set; }

	public bool IsSent { get; set; }

	public DateTime CreatedOn { get; set; }

	public void MarkAsRead() => IsRead = true;

	public void MarkAsSent() => IsSent = true;
}