using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Interfaces;
using SocialLink.Users.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialLink.Users.Application.Services;

internal class AuthManager : IAuthManager
{
	private readonly JwtSettings _settings;
	private readonly byte[] _key;
	private readonly int _tokenDuration = Constants.TOKEN_DURATION_HOURS;

	public AuthManager(IOptions<JwtSettings> settings)
	{
		_settings = settings.Value;
		Guard.Against.Null(_settings);
		Guard.Against.Null(_settings.SigningKey);
		Guard.Against.Null(_settings.Issuer);
		Guard.Against.Null(_settings.Audience);
		_key = Encoding.ASCII.GetBytes(_settings.SigningKey);
	}

	public string GenerateJwtToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

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
			Expires = DateTime.UtcNow.AddHours(_tokenDuration),
			Audience = _settings.Audience,
			Issuer = _settings.Issuer,
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature),
			Claims = claims.ToDictionary(claim => claim.Type, claim => (object)claim.Value)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}

	public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

	public bool VerifyPassword(string password, string storedPassword) => BCrypt.Net.BCrypt.Verify(password, storedPassword);
}