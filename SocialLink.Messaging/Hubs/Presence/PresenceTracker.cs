namespace SocialLink.Messaging.Hubs.Presence;

public class PresenceTracker : IPresenceTracker
{
	private static readonly Dictionary<string, HashSet<string>> _onlineUsers = new();
	private static readonly Dictionary<string, string> _connectionUserAgents = new();


	public Task<bool> UserConnectedAsync(string userId, string connectionId)
	{
		bool isFirstConnection;

		lock (_onlineUsers)
		{
			if (_onlineUsers.TryGetValue(userId, out var connections))
			{
				isFirstConnection = connections.Add(connectionId) && connections.Count is 1;
			}
			else
			{
				_onlineUsers[userId] = new HashSet<string> { connectionId };
				isFirstConnection = true;
			}
		}

		return Task.FromResult(isFirstConnection);
	}

	public Task<bool> UserDisconnectedAsync(string userId, string connectionId)
	{
		bool isLastConnection = false;

		lock (_onlineUsers)
		{
			if (!_onlineUsers.TryGetValue(userId, out var connections))
				return Task.FromResult(false);

			connections.Remove(connectionId);
			if (connections.Count is 0)
			{
				_onlineUsers.Remove(userId, out var removedConnections);
				isLastConnection = true;
			}
		}

		return Task.FromResult(isLastConnection);
	}

	public Task<IReadOnlyCollection<string>> GetOnlineUsersAsync(IEnumerable<string> userIds)
	{
		lock (_onlineUsers)
		{
			var online = _onlineUsers.Keys.Intersect(userIds).ToList();
			return Task.FromResult<IReadOnlyCollection<string>>(online);
		}
	}

	public void AddClientInfo(string connectionId, string clientInfo)
	{
		lock (_connectionUserAgents)
		{
			_connectionUserAgents[connectionId] = clientInfo;
		}
	}

	public void RemoveClientInfo(string connectionId)
	{
		lock (_connectionUserAgents)
		{
			_connectionUserAgents.Remove(connectionId);
		}
	}
}