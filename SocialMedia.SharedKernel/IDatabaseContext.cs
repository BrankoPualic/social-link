namespace SocialMedia.SharedKernel;

public interface IAppDatabaseContext : IDatabaseContextBase
{ }

public interface IDatabaseContextBase
{
	IIdentityUser CurrentUser { get; }

	int SaveChanges();

	Task<int> SaveChangesAsync(CancellationToken ct = default);
}