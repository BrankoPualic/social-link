namespace SocialMedia.Blobs.Endpoints;

internal sealed record GetBlobsRequest(List<Guid> BlobIds);