using SocialLink.SharedKernel.Domain;

namespace SocialLink.SharedKernel;

public class MongoDatabaseContext : IMongoDatabaseContext
{
	public IIdentityUser CurrentUser { get; private set; }

	public MongoDatabaseContext(IIdentityUser currentUser) => CurrentUser = currentUser;

	public async Task<T> ExecuteWithAuditAsync<T>(T entity, bool isNew, Func<T, Task> mongoOperation)
		where T : IAuditedDomainModel
	{
		var (now, userId) = GetAuditInfo();

		entity.LastChangedBy = userId;
		entity.LastChangedOn = now;

		if (isNew)
		{
			entity.CreatedBy = userId;
			entity.CreatedOn = now;
		}

		await mongoOperation(entity);

		return entity;
	}

	// private
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