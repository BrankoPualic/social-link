using SocialLink.Messaging.Enumerators;
using SocialLink.Users.Contracts;

namespace SocialLink.Messaging.Hubs.Message;

public class MessageResponse
{
	public Guid Id { get; set; }

	public Guid ChatGroupId { get; set; }

	public Guid UserId { get; set; }

	public eMessageType? Type { get; set; }

	public string Content { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime LastChangedOn { get; set; }

	public UserContractDto User { get; set; }
}