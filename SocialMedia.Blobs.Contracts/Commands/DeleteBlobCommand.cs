using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Blobs.Contracts.Commands;

public sealed record DeleteBlobCommand(Guid BlobId) : Command<Guid>;