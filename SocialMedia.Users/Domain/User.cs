using SocialMedia.SharedKernel.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Users.Domain;

internal class User : AuditedDomainModel<Guid>
{
	public string Username { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }

	[InverseProperty(nameof(UserRole.User))]
	public virtual ICollection<UserRole> Roles { get; set; } = [];
}