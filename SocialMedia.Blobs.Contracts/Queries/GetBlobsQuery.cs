using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Blobs.Contracts.Queries;
public sealed record GetBlobsQuery(List<Guid> BlobIds) : Query<List<BlobDto>>;