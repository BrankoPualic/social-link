using MediatR;
using Microsoft.AspNetCore.SignalR;
using SocialLink.Messaging.Hubs.Message;

namespace SocialLink.Messaging.Events;

public sealed record CreateMessageEvent(MessageResponse Message) : INotification;

public class CreateMessageEventHandler(IHubContext<MessageHub, IMessageHub> hubContext) : INotificationHandler<CreateMessageEvent>
{
	public async Task Handle(CreateMessageEvent notification, CancellationToken cancellationToken)
	{
		await hubContext.Clients.Group(notification.Message.ChatGroupId.ToString()).NewMessage(notification.Message);
	}
}