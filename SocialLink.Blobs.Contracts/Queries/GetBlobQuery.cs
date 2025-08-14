using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Blobs.Contracts.Queries;
public sealed record GetBlobQuery(Guid BlobId) : Query<BlobDto>;