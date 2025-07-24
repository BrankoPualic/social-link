using System.Security.Claims;

namespace SocialMedia.Web.Extensions;

public static class ClaimsExtensions
{
	public static Guid GetId(this IEnumerable<Claim> claims) => Guid.Parse(claims.GetClaim("UserId"));

	public static string GetEmail(this IEnumerable<Claim> claims) => claims.GetClaim("Email");

	public static string GetUsername(this IEnumerable<Claim> claims) => claims.GetClaim("Username");

	public static string GetRoles(this IEnumerable<Claim> claims) => claims.GetClaim("Roles");

	public static string GetClaim(this IEnumerable<Claim> claims, string claimName) => claims.SingleOrDefault(i => i.Type.Equals(claimName)).Value;
}