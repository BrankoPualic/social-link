namespace SocialMedia.Blobs.Contracts.Dtos;

public sealed record UploadResult(Guid BlobId, Func<Task> Cleanup);