using SocialLink.Users.Application.Interfaces;

namespace SocialLink.Users.Application.Services;

internal class AuthManager : IAuthManager
{
	public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

	public bool VerifyPassword(string password, string storedPassword) => BCrypt.Net.BCrypt.Verify(password, storedPassword);
}
