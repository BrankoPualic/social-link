using Microsoft.EntityFrameworkCore;
using SocialMedia.SharedKernel;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Data;

internal sealed class UserDatabaseContext : DbContext, IDatabaseContext
{
	public IIdentityUser CurrentUser { get; private set; }

	public UserDatabaseContext(IIdentityUser currentUser) => CurrentUser = currentUser;

	public UserDatabaseContext(DbContextOptions options, IIdentityUser currentUser) : base(options) => CurrentUser = currentUser;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("user");

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDatabaseContext).Assembly);

		foreach (var entity in modelBuilder.Model.GetEntityTypes())
		{
			entity.SetTableName(entity.DisplayName());
		}
	}

	public new void SaveChanges() => base.SaveChanges();

	public new async Task<int> SaveChangesAsync(CancellationToken ct = default) => await base.SaveChangesAsync(ct);

	#region DbSets

	public DbSet<User> Users { get; set; }

	public DbSet<UserRole> UserRoles { get; set; }

	public DbSet<UserLogin> Logins { get; set; }

	#endregion DbSets
}