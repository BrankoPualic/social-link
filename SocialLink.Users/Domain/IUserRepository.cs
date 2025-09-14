namespace SocialLink.Users.Domain;

internal interface IUserRepository
{
	Task<User> GetByEmailAsync(string email, CancellationToken ct);

	void CreateLoginLog(Guid userId);

	void CreateMedia(Guid userId, Guid blobId, eUserMedia type);

	Task<bool> IsUsernameTaken(string username, CancellationToken ct);

	Task<bool> IsEmailRegistered(string email, CancellationToken ct);
}