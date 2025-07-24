using Microsoft.EntityFrameworkCore;
using SocialMedia.SharedKernel;

namespace SocialMedia.Users.Data;

internal sealed class UserDbContext : DbContext
{
	public IIdentityUser CurrentUser { get; private set; }

	public UserDbContext(IIdentityUser currentUser) => CurrentUser = currentUser;

	public UserDbContext(DbContextOptions options, IIdentityUser currentUser) : base(options) => CurrentUser = currentUser;

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