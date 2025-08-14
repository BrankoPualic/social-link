using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialLink.Users.Domain;

internal class NotificationPreference : DomainModel<Guid>
{
	public Guid UserId { get; set; }

	public eNotificationType NotificationTypeId { get; set; }

	public bool IsMuted { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User User { get; set; }
}