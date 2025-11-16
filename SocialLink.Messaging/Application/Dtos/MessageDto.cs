using SocialLink.Messaging.Domain.Document;
using SocialLink.Users.Contracts;
using System.Linq.Expressions;

namespace SocialLink.Messaging.Application.Dtos;

internal class MessageDto
{
	public Guid Id { get; set; }

	public Guid ChatGroupId { get; set; }

	public Guid UserId { get; set; }

	public string Content { get; set; }

	public DateTime? CreatedOn { get; set; }

	public DateTime? LastChangedOn { get; set; }

	public UserContractDto User { get; set; }

	public static Expression<Func<Message, MessageDto>> Projection => _ => new()
	{
		Id = _.Id,
		ChatGroupId = _.ChatGroupId,
		UserId = _.UserId,
		Content = _.Content,
		CreatedOn = _.CreatedOn,
		LastChangedOn = _.LastChangedOn,
	};

	public void ToModel(Message model)
	{
		model.ChatGroupId = ChatGroupId;
		model.UserId = UserId;
		model.Content = Content.Trim();
	}
}