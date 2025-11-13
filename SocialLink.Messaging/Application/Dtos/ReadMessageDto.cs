namespace SocialLink.Messaging.Application.Dtos;

internal class ReadMessageDto
{
	public Guid LastMessageId { get; set; }

	public Guid ChatGroupId { get; set; }

	public Guid UserId { get; set; }
}