using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Users.Data;

internal sealed class UserDbContext : DbContext
{
	public UserDbContext()
	{
	}

	public UserDbContext(DbContextOptions options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("user");

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);

		foreach (var entity in modelBuilder.Model.GetEntityTypes())
		{
			entity.SetTableName(entity.DisplayName());
		}
	}
}