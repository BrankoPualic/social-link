using SocialLink.Common.Data;
using SocialLink.SharedKernel.Domain;

namespace SocialLink.Common.Application;

public abstract class EFUseCase(IEFDatabaseContext db)
{
	public IIdentityUser CurrentUser => db.CurrentUser;
}

public abstract class MongoUseCase
{ }