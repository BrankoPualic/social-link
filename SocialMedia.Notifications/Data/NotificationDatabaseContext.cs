using Microsoft.EntityFrameworkCore;
using SocialMedia.Notifications.Domain;
using SocialMedia.SharedKernel;
using SocialMedia.SharedKernel.Data;

namespace SocialMedia.Notifications.Data;

internal sealed class NotificationDatabaseContext : AppDatabaseContext, INotificationDatabaseContext
{
	public NotificationDatabaseContext(IIdentityUser currentUser) : base(currentUser)
	{ }

	public NotificationDatabaseContext(DbContextOptions<NotificationDatabaseContext> options, IIdentityUser currentUser) : base(options, currentUser)
	{ }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("notification");

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDatabaseContext).Assembly);

		foreach (var entity in modelBuilder.Model.GetEntityTypes())
		{
			entity.SetTableName(entity.DisplayName());
		}
	}

	public DbSet<Notification> Notifications { get; set; }
}