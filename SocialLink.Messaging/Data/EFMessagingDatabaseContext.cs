using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Data;
using SocialLink.Messaging.Domain.Relational;
using SocialLink.SharedKernel.Domain;

namespace SocialLink.Messaging.Data;

internal sealed class EFMessagingDatabaseContext : EFDatabaseContext, IEFMessagingDatabaseContext
{
	public EFMessagingDatabaseContext(IIdentityUser currentUser) : base(currentUser)
	{ }

	public EFMessagingDatabaseContext(DbContextOptions options, IIdentityUser currentUser) : base(options, currentUser)
	{ }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("messaging");

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFMessagingDatabaseContext).Assembly);

		foreach (var entity in modelBuilder.Model.GetEntityTypes())
		{
			entity.SetTableName(entity.DisplayName());
		}
	}

	#region DbSets

	public DbSet<ChatGroup> ChatGroups { get; set; }

	public DbSet<ChatGroupUser> ChatGroupUsers { get; set; }

	#endregion DbSets
}