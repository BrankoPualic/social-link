using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Interfaces;
using SocialLink.Users.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialLink.Users.Application.Services;

internal class AuthTokenProcessor : IAuthTokenProcessor
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly JwtSettings _settings;
	private readonly byte[] _key;
	private readonly int _tokenDuration = Constants.TOKEN_DURATION_IN_MINUTES;

	public AuthTokenProcessor(IOptions<JwtSettings> settings, IHttpContextAccessor httpContextAccessor)
	{
		_settings = settings.Value;
		Guard.Against.Null(_settings);
		Guard.Against.Null(_settings.SigningKey);
		Guard.Against.Null(_settings.Issuer);
		Guard.Against.Null(_settings.Audience);
		_key = Encoding.ASCII.GetBytes(_settings.SigningKey);
		_httpContextAccessor = httpContextAccessor;
	}

	public (string jwtToken, DateTime expiresAt) GenerateJwtToken(User user)
	{
		var expiration = DateTime.UtcNow.AddMinutes(_tokenDuration);

		var claims = new List<Claim>()
		{
			new(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(ClaimTypes.Email, user.Email),
			new(ClaimTypes.Name, user.Username)
		};

		var roles = user.Roles.Select(_ => ((int)_.RoleId).ToString()).ToList();

		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


		var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature);

		var token = new JwtSecurityToken(
			issuer: _settings.Issuer,
			audience: _settings.Audience,
			expires: expiration,
			claims: claims,
			signingCredentials: signingCredentials);

		var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

		return (jwtToken, expiration);
	}

	public string GenerateRefreshToken()
	{
		var randomNumber = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	public void WriteTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration)
	{
		_httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, token, new CookieOptions
		{
			HttpOnly = true,
			Expires = expiration,
			IsEssential = true,
			Secure = true,
			SameSite = SameSiteMode.Strict,
		});
	}

	public void DeleteCookie(string cookieName)
	{
		_httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName, new CookieOptions
		{
			HttpOnly = true,
			Secure = true,
			SameSite = SameSiteMode.Strict
		});
	}
}