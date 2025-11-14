using SocialLink.Messaging.Domain.Relational;
using SocialLink.Users.Contracts;
using System.Linq.Expressions;

namespace SocialLink.Messaging.Application.Dtos;

internal class ConversationDto
{
	public Guid Id { get; set; }

	public DateTime? LastMessageOn { get; set; }

	public string LastMessagePreview { get; set; }

	public UserContractDto User { get; set; }

	public static Expression<Func<ChatGroup, ConversationDto>> Projection => _ => new()
	{
		Id = _.Id,
		LastMessageOn = _.LastMessageOn,
		LastMessagePreview = _.LastMessagePreview
	};
}