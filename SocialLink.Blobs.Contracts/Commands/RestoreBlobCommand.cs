using SocialLink.Common.Application;

namespace SocialLink.Blobs.Contracts.Commands;

public sealed record RestoreBlobCommand(Guid BlobId) : Command<Guid>;