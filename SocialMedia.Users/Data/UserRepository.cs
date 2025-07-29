using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Data;

internal class UserRepository(IUserDatabaseContext db) : IUserRepository
{
	public Task<User> GetByEmailAsync(string email) =>
		db.Users
		.Include(_ => _.Roles)
		.Where(_ => _.Email == email)
		.FirstOrDefaultAsync();

	public void CreateLoginLog(Guid userId) =>
		db.Logins.Add(new()
		{
			Id = Guid.NewGuid(),
			UserId = userId,
			LoggedOn = DateTime.UtcNow,
		});

	public void CreateMedia(Guid userId, Guid blobId) =>
		db.Media.Add(new()
		{
			UserId = userId,
			BlobId = blobId,
			IsActive = true,
			UploadedOn = DateTime.UtcNow,
		});
}