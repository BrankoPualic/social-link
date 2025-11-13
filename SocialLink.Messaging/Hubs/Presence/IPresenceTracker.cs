namespace SocialLink.Messaging.Hubs.Presence;

public interface IPresenceTracker
{
	Task<bool> UserConnectedAsync(string userId, string connectionId);

	Task<bool> UserDisconnectedAsync(string userId, string connectionId);

	Task<IReadOnlyCollection<string>> GetOnlineUsersAsync(IEnumerable<string> userIds);

	void AddClientInfo(string connectionId, string clientInfo);

	void RemoveClientInfo(string connectionId);
}