using Microsoft.EntityFrameworkCore;
using SocialMedia.SharedKernel;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users;

internal interface IDatabaseContext : IAppDatabaseContext
{
	DbSet<User> Users { get; }

	DbSet<UserRole> UserRoles { get; }

	DbSet<UserLogin> Logins { get; }
}