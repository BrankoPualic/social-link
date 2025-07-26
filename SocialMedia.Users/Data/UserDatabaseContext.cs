using Microsoft.EntityFrameworkCore;
using SocialMedia.SharedKernel;
using SocialMedia.SharedKernel.Data;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Data;

internal sealed class UserDatabaseContext : AppDatabaseContext, IUserDatabaseContext
{
	public UserDatabaseContext(IIdentityUser currentUser) : base(currentUser)
	{ }

	public UserDatabaseContext(DbContextOptions options, IIdentityUser currentUser) : base(options, currentUser)
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

	#endregion DbSets
}