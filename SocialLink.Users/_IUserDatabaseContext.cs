using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Data;
using SocialLink.Users.Domain;

namespace SocialLink.Users;

internal interface IUserDatabaseContext : IEFDatabaseContext
{
	DbSet<User> Users { get; }

	DbSet<UserRole> UserRoles { get; }

	DbSet<UserLogin> Logins { get; }

	DbSet<UserFollow> Follows { get; }

	DbSet<NotificationPreference> NotificationPreferences { get; }

	DbSet<UserMedia> Media { get; }
}