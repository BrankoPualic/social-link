using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Blobs.Contracts.Queries;
public sealed record GetBlobsQuery(List<Guid> BlobIds) : Query<List<BlobDto>>, ICacheableQuery
{
	public string CacheKey => $"blobs:{string.Join(',', BlobIds)}";

	public TimeSpan? CacheDuration => TimeSpan.FromMinutes(55);
}