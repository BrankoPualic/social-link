using SocialLink.Common.Application;

namespace SocialLink.Users.Contracts;

public sealed record GetUsersContractQuery(List<Guid> UserIds) : Query<List<UserContractDto>>, ICacheableQuery
{
	public string CacheKey => $"users-contract:{string.Join(',', UserIds)}";

	public TimeSpan? CacheDuration => TimeSpan.FromMinutes(55);
}