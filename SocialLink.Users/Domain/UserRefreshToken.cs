using SocialLink.SharedKernel.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialLink.Users.Domain;

internal class UserRefreshToken : AuditedDomainModel<Guid>
{
	public Guid UserId { get; set; }

	public string Token { get; set; }

	public DateTime TokenExpiresAt { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User User { get; set; }
}