using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Data;
using SocialLink.SharedKernel.Domain;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Data;

internal sealed class UserDatabaseContext : EFDatabaseContext, IUserDatabaseContext
{
	public UserDatabaseContext(IIdentityUser currentUser) : base(currentUser)
	{ }

	public UserDatabaseContext(DbContextOptions<UserDatabaseContext> options, IIdentityUser currentUser) : base(options, currentUser)
	{ }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("user");

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDatabaseContext).Assembly);

		foreach (var entity in modelBuilder.Model.GetEntityTypes())
		{
			entity.SetTableName(entity.DisplayName());
		}
	}

	#region DbSets

	public DbSet<User> Users { get; set; }

	public DbSet<UserRole> UserRoles { get; set; }

	public DbSet<UserLogin> Logins { get; set; }

	public DbSet<UserFollow> Follows { get; set; }

	public DbSet<NotificationPreference> NotificationPreferences { get; set; }

	public DbSet<UserMedia> Media { get; set; }

	#endregion DbSets
}