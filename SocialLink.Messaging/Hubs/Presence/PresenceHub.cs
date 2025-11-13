using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SocialLink.Users.Contracts;

namespace SocialLink.Messaging.Hubs.Presence;

public interface IUserPresence
{
	Task UserIsOnline(string userId);

	Task UserIsOffline(string userId);

	Task FollowingOnlineList(IReadOnlyCollection<string> onlineFollowing);
}

[Authorize]
public sealed class PresenceHub(IPresenceTracker presenceTracker, IMediator mediator) : Hub<IUserPresence>
{
	public override async Task OnConnectedAsync()
	{
		var userId = Context.UserIdentifier;
		if (!Guid.TryParse(userId, out var userIdParsed))
			throw new InvalidOperationException("Invalid user tried to establish connection.\nUSER_ID = {userId}");

		var followers = (await mediator.Send(new GetFollowerIdsQuery(userIdParsed))).Data;
		var following = (await mediator.Send(new GetFollowingIdsQuery(userIdParsed))).Data;

		var isOnline = await presenceTracker.UserConnectedAsync(userId, Context.ConnectionId);

		if (isOnline)
		{
			var followersAsString = followers.Select(_ => _.ToString()).ToList();
			await Clients.Users(followersAsString).UserIsOnline(userId);
		}

		var followingAsString = following.Select(_ => _.ToString()).ToList();
		var onlineFollowing = await presenceTracker.GetOnlineUsersAsync(followingAsString);
		await Clients.Caller.FollowingOnlineList(onlineFollowing);
	}

	public override async Task OnDisconnectedAsync(Exception exception)
	{
		var userId = Context.UserIdentifier;
		if (!Guid.TryParse(userId, out var userIdParsed))
			throw new InvalidOperationException($"Invalid user tried to revoke connection.\nUSER_ID = {userId}");

		var followers = (await mediator.Send(new GetFollowerIdsQuery(userIdParsed))).Data;

		var isOffline = await presenceTracker.UserDisconnectedAsync(userId, Context.ConnectionId);
		if (isOffline)
		{
			var followersAsString = followers.Select(_ => _.ToString()).ToList();
			await Clients.Users(followersAsString).UserIsOffline(userId);
		}

		presenceTracker.RemoveClientInfo(Context.ConnectionId);

		await base.OnDisconnectedAsync(exception);
	}

	public void RegisterClientInfo(string clientInfo) => presenceTracker.AddClientInfo(Context.ConnectionId, clientInfo);
}