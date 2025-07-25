using SocialMedia.SharedKernel.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Users.Domain;

internal class User : AuditedDomainModel<Guid>
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
	public string FullName { get; private set; }

	public string Username { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }

	public eGender GenderId { get; set; }

	public bool IsPrivate { get; set; }

	public bool IsActive { get; set; }

	public bool IsLocked { get; set; }

	[InverseProperty(nameof(UserRole.User))]
	public virtual ICollection<UserRole> Roles { get; set; } = [];
}