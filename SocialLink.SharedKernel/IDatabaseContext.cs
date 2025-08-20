using SocialLink.SharedKernel.Domain;

namespace SocialLink.SharedKernel;

public interface IDatabaseContextBase
{
	IIdentityUser CurrentUser { get; }
}

public interface IEFDatabaseContext : IDatabaseContextBase
{
	bool HasChanges();

	void ClearChanges();

	int SaveChanges(bool audit = true);

	Task<int> SaveChangesAsync(bool audit = true, CancellationToken ct = default);
}

public interface IMongoDatabaseContext : IDatabaseContextBase
{
	Task<T> ExecuteWithAuditAsync<T>(T entity, bool isNew, Func<T, Task> mongoOperation)
		where T : IAuditedDomainModel;
}