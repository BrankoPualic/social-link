namespace SocialLink.SharedKernel;

public interface IDatabaseContextBase
{ }

public interface IEFDatabaseContext : IDatabaseContextBase
{
	IIdentityUser CurrentUser { get; }

	bool HasChanges();

	void ClearChanges();

	int SaveChanges(bool audit = true);

	Task<int> SaveChangesAsync(bool audit = true, CancellationToken ct = default);
}

public interface IMongoDatabaseContext : IDatabaseContextBase
{ }