using SocialLink.Messaging.Enumerators;

namespace SocialLink.Messaging.Domain;

internal interface IChatGroupRepository
{
	void CreateMedia(Guid chatGroupId, Guid blobId, eChatGroupMedia type);
}