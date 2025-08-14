using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Blobs.Contracts.Queries;
public sealed record GetBlobsQuery(List<Guid> BlobIds) : Query<List<BlobDto>>;