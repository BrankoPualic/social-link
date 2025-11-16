namespace SocialLink.Messaging.Application.Dtos;

internal class ConversationCreateDto
{
	public string Name { get; set; }

	public List<Guid> Users { get; set; } = [];
}