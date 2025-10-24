using System.Security.Claims;

namespace SocialLink.Web.Extensions;

public static class ClaimsExtensions
{
	public static Guid GetId(this IEnumerable<Claim> claims) => Guid.Parse(claims.GetClaim(ClaimTypes.NameIdentifier));

	public static string GetEmail(this IEnumerable<Claim> claims) => claims.GetClaim(ClaimTypes.Email);

	public static string GetUsername(this IEnumerable<Claim> claims) => claims.GetClaim(ClaimTypes.Name);

	public static string GetRoles(this IEnumerable<Claim> claims) => claims.GetClaim(ClaimTypes.Role);

	public static string GetClaim(this IEnumerable<Claim> claims, string claimName) => claims.SingleOrDefault(i => i.Type.Equals(claimName)).Value;
}