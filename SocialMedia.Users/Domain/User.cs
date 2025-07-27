using SocialMedia.SharedKernel.Domain;

namespace SocialMedia.Users.Domain;

internal class User : AuditedDomainModel<Guid>
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string FullName { get; set; }

	public string Username { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }

	public eGender GenderId { get; set; }

	public DateTime? DateOfBirth { get; set; }

	public bool IsPrivate { get; set; }

	public bool IsActive { get; set; }

	public bool IsLocked { get; set; }

	public virtual ICollection<UserRole> Roles { get; set; } = [];

	public virtual ICollection<UserLogin> Logins { get; set; } = [];

	public virtual ICollection<UserFollow> Following { get; set; } = [];

	public virtual ICollection<UserFollow> Followers { get; set; } = [];

	public virtual ICollection<NotificationPreference> NotificationPreferences { get; set; } = [];
}