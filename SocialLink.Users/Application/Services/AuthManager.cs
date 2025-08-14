using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialLink.Users.Application.Interfaces;
using SocialLink.Users.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialLink.Users.Application.Services;

internal class AuthManager(IOptions<JwtSetting> jwtSetting) : IAuthManager
{
	private readonly JwtSetting _jwtSetting = jwtSetting.Value;

	public string GenerateJwtToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var jwtSecrets = new
		{
			Key = Encoding.UTF8.GetBytes(_jwtSetting.SecretKey),
			_jwtSetting.Duration,
		};

		var claims = new List<Claim>()
		{
			new("UserId", user.Id.ToString()),
			new("Username", user.Username),
			new("Email", user.Email)
		};

		var roles = user.Roles.Select(_ => ((int)_.RoleId).ToString()).ToList();

		claims.AddRange(roles.Select(role => new Claim("Roles", role)));

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Expires = DateTime.UtcNow.AddDays(jwtSecrets.Duration),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtSecrets.Key), SecurityAlgorithms.HmacSha512Signature),
			Claims = claims.ToDictionary(claim => claim.Type, claim => (object)claim.Value)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}

	public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

	public bool VerifyPassword(string password, string storedPassword) => BCrypt.Net.BCrypt.Verify(password, storedPassword);
}