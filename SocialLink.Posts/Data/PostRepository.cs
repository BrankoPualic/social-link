using SocialLink.Posts.Domain;

namespace SocialLink.Posts.Data;
internal class PostRepository(IPostDatabaseContext db) : IPostRepository
{
	public void CreateMedia(Guid postId, Guid blobId, ePostMedia type, int order) =>
		db.Media.Add(new()
		{
			PostId = postId,
			BlobId = blobId,
			IsActive = true,
			UploadedOn = DateTime.UtcNow,
			Type = type,
			Order = order
		});
}
