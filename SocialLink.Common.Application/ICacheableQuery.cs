namespace SocialLink.Common.Application;

public interface ICacheableQuery
{
	string CacheKey { get; }
	TimeSpan? CacheDuration { get; }
}