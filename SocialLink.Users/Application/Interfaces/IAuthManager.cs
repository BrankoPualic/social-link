using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.Interfaces;

internal interface IAuthManager
{
	string GenerateJwtToken(User user);

	string HashPassword(string password);

	bool VerifyPassword(string password, string storedPassword);
}