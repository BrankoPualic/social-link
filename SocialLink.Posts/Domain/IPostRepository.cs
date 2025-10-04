namespace SocialLink.Posts.Domain;
internal interface IPostRepository
{
	void CreateMedia(Guid postId, Guid blobId, ePostMedia type, int order);
}
