using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Blobs.Contracts.Queries;
public sealed record GetBlobQuery(Guid BlobId) : Query<BlobDto>;