namespace SocialMedia.SharedKernel;

public interface IAppDatabaseContext : IDatabaseContextBase
{ }

public interface IDatabaseContextBase
{
	IIdentityUser CurrentUser { get; }

	bool HasChanges();

	void ClearChanges();

	int SaveChanges(bool audit = true);

	Task<int> SaveChangesAsync(bool audit = true, CancellationToken ct = default);
}