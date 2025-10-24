using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.Interfaces;

internal interface IAuthTokenProcessor
{
	(string jwtToken, DateTime expiresAt) GenerateJwtToken(User user);

	string GenerateRefreshToken();

	void WriteTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);

	void DeleteCookie(string cookieName);
}