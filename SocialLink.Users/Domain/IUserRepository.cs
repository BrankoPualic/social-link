namespace SocialLink.Users.Domain;

internal interface IUserRepository
{
	Task<User> GetByEmailAsync(string email);

	void CreateLoginLog(Guid userId);

	void CreateMedia(Guid userId, Guid blobId);
}