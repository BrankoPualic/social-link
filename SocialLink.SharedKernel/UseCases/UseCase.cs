namespace SocialLink.SharedKernel.UseCases;

public abstract class EFUseCase(IEFDatabaseContext db)
{
	public IIdentityUser CurrentUser => db.CurrentUser;
}

public abstract class MongoUseCase
{ }