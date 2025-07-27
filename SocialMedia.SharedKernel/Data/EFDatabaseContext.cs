using Microsoft.EntityFrameworkCore;
using SocialMedia.SharedKernel.Domain;

namespace SocialMedia.SharedKernel.Data;

public class EFDatabaseContext : DbContext, IEFDatabaseContext
{
	public IIdentityUser CurrentUser { get; private set; }

	public EFDatabaseContext(IIdentityUser currentUser) => CurrentUser = currentUser;

	public EFDatabaseContext(DbContextOptions options, IIdentityUser currentUser) : base(options) => CurrentUser = currentUser;

	public bool HasChanges() => ChangeTracker.HasChanges();

	public void ClearChanges() => ChangeTracker.Clear();

	public new int SaveChanges(bool audit = true)
	{
		RunAudit(audit);

		return base.SaveChanges();
	}

	public new async Task<int> SaveChangesAsync(bool audit = true, CancellationToken ct = default)
	{
		RunAudit(audit);

		var result = await base.SaveChangesAsync(ct);

		return result;
	}

	// private

	private void RunAudit(bool audit)
	{
		if (!audit || !HasChanges())
			return;

		var changedEntries = ChangeTracker.Entries()
			.Where(_ => _.State.In(EntityState.Modified, EntityState.Added)
			&& _.Entity.GetType().GetInterface(nameof(IAuditedDomainModel)) != null)
			.ToList();

		var (now, userId) = GetAuditInfo();

		foreach (var changedEntry in changedEntries)
		{
			var entity = (IAuditedDomainModel)changedEntry.Entity;

			entity.LastChangedBy = userId;
			entity.LastChangedOn = now;

			if (changedEntry.State == EntityState.Added)
			{
				entity.CreatedBy = userId;
				entity.CreatedOn = now;
			}
		}
	}

	private (DateTime now, Guid userId) GetAuditInfo()
	{
		var now = DateTime.UtcNow;

		var userId = CurrentUser.Id;
		if (userId == default)
		{
			userId = Guid.Parse(Constants.SYSTEM_USER);
		}

		return (now, userId);
	}
}