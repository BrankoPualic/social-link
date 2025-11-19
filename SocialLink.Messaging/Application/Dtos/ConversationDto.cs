using SocialLink.Messaging.Domain.Relational;
using SocialLink.Users.Contracts;
using System.Linq.Expressions;

namespace SocialLink.Messaging.Application.Dtos;

internal class ConversationDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public DateTime? LastMessageOn { get; set; }

	public string LastMessagePreview { get; set; }

	public UserContractDto User { get; set; }

	public string GroupImageUrl { get; set; }

	public bool? IsGroup { get; set; }

	public static Expression<Func<ChatGroup, ConversationDto>> Projection => _ => new()
	{
		Id = _.Id,
		Name = _.Name,
		LastMessageOn = _.LastMessageOn,
		LastMessagePreview = _.LastMessagePreview,
		IsGroup = _.IsGroup
	};
}