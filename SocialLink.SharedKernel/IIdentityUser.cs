namespace SocialLink.SharedKernel;

public interface IIdentityUser
{
	Guid Id { get; }

	string Email { get; }

	string Username { get; }

	List<eSystemRole> Roles { get; }

	bool IsAuthenticated { get; }

	bool HasRole(List<eSystemRole> roles);
}