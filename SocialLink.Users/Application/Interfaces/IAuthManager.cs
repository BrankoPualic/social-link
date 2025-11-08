namespace SocialLink.Users.Application.Interfaces;

internal interface IAuthManager
{
	string HashPassword(string password);

	bool VerifyPassword(string password, string storedPassword);
}
