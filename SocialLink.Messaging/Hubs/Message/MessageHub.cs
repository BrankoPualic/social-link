using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SocialLink.SharedKernel.Extensions;

namespace SocialLink.Messaging.Hubs.Message;

public interface IMessageHub
{
	Task NewMessage(MessageResponse response);

	Task UserIsTyping(string username);

	Task UserStoppedTyping();
}

[Authorize]
public class MessageHub : Hub<IMessageHub>
{
	public override async Task OnConnectedAsync()
	{
		var chatGroupId = Context.GetHttpContext().Request.Query["chatGroupId"];
		var connectionId = Context.ConnectionId;

		await Groups.AddToGroupAsync(connectionId, chatGroupId);
	}
	public override async Task OnDisconnectedAsync(Exception exception)
	{
		var chatGroupId = Context.GetHttpContext().Request.Query["chatGroupId"];
		var connectionId = Context.ConnectionId;

		await Groups.RemoveFromGroupAsync(connectionId, chatGroupId);

		await base.OnDisconnectedAsync(exception);
	}

	public async Task StartedTyping(Guid chatGroupId)
	{
		var username = Context.User.Claims.GetUsername();
		await Clients.OthersInGroup(chatGroupId.ToString()).UserIsTyping(username);
	}

	public async Task StoppedTyping(Guid chatGroupId)
	{
		await Clients.OthersInGroup(chatGroupId.ToString()).UserStoppedTyping();
	}
}