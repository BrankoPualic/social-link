namespace SocialLink.SharedKernel;

public interface ICacheableQuery
{
	string CacheKey { get; }
	TimeSpan? CacheDuration { get; }
}