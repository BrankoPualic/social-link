using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SocialLink.Common.Application;

namespace SocialLink.Web.Behaviors;

public class CachingBehavior<TRequest, TResponse>(IMemoryCache cache) : IPipelineBehavior<TRequest, TResponse>
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (request is not ICacheableQuery cacheable)
			return await next();

		if (cache.TryGetValue(cacheable.CacheKey, out TResponse cached))
			return cached;

		var response = await next();

		if (cacheable.CacheDuration.HasValue)
			cache.Set(cacheable.CacheKey, response, cacheable.CacheDuration.Value);

		return response;
	}
}
