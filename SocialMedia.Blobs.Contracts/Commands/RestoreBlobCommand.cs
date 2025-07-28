using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Blobs.Contracts.Commands;

public sealed record RestoreBlobCommand(Guid BlobId) : Command<Guid>;