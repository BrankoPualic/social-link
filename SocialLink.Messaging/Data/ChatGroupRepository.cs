using SocialLink.Messaging.Domain;
using SocialLink.Messaging.Enumerators;

namespace SocialLink.Messaging.Data;

internal class ChatGroupRepository(IEFMessagingDatabaseContext db) : IChatGroupRepository
{
	public void CreateMedia(Guid chatGroupId, Guid blobId, eChatGroupMedia type) =>
		db.Media.Add(new()
		{
			ChatGroupId = chatGroupId,
			BlobId = blobId,
			Type = type,
			IsActive = true,
			UploadedOn = DateTime.UtcNow,
		});
}