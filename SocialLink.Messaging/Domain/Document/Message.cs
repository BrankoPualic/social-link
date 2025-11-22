using SocialLink.Messaging.Enumerators;
using SocialLink.SharedKernel.Domain;

namespace SocialLink.Messaging.Domain.Document;

internal class Message : MongoDomainModel<Guid>
{
	public Guid ChatGroupId { get; set; }

	public Guid UserId { get; set; }

	public eMessageType? Type { get; set; }

	public string Content { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime LastChangedOn { get; set; }
}