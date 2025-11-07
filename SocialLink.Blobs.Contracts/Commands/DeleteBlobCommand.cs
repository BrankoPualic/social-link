using SocialLink.Common.Application;

namespace SocialLink.Blobs.Contracts.Commands;

public sealed record DeleteBlobCommand(Guid BlobId) : Command<Guid>;