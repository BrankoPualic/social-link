using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Domain;

namespace SocialLink.Notifications.Domain;

internal class Notification : MongoDomainModel<Guid>
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