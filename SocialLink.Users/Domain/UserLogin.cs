using SocialLink.SharedKernel.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialLink.Users.Domain;

internal class UserLogin : DomainModel<Guid>
{
	public Guid UserId { get; set; }

	public DateTime LoggedOn { get; set; }

	[ForeignKey(nameof(UserId))]
	public virtual User User { get; set; }
}