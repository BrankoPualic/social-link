using Microsoft.EntityFrameworkCore;
using SocialLink.Users.Domain;
using SocialLink.Users.Enumerators;

namespace SocialLink.Users.Data;

internal class UserRepository(IUserDatabaseContext db) : IUserRepository
{
	public Task<User> GetByEmailAsync(string email, CancellationToken ct) =>
		db.Users
		.Include(_ => _.Roles)
		.Where(_ => _.Email == email)
		.FirstOrDefaultAsync(ct);

	public void CreateLoginLog(Guid userId) =>
		db.Logins.Add(new()
		{
			Id = Guid.NewGuid(),
			UserId = userId,
			LoggedOn = DateTime.UtcNow,
		});

	public void CreateMedia(Guid userId, Guid blobId, eUserMedia type) =>
		db.Media.Add(new()
		{
			UserId = userId,
			BlobId = blobId,
			IsActive = true,
			UploadedOn = DateTime.UtcNow,
			Type = type
		});

	public Task<bool> IsUsernameTaken(string username, CancellationToken ct) =>
		db.Users.AnyAsync(_ => _.Username == username, ct);

	public Task<bool> IsEmailRegistered(string email, CancellationToken ct) =>
		db.Users.AnyAsync(_ => _.Email == email, ct);
}