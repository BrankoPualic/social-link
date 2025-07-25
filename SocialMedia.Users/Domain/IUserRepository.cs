namespace SocialMedia.Users.Domain;

internal interface IUserRepository
{
	Task<User> GetByEmailAsync(string email);

	void CreateLoginLog(Guid userId);
}