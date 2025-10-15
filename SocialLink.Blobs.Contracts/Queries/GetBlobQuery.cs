using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Blobs.Contracts.Queries;
public sealed record GetBlobQuery(Guid BlobId) : Query<BlobDto>, ICacheableQuery
{
	public string CacheKey => $"blob:{BlobId}";

	public TimeSpan? CacheDuration => TimeSpan.FromMinutes(55);
}