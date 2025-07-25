namespace SocialMedia.SharedKernel.UseCases;

public abstract class UseCase(IAppDatabaseContext db)
{
	public IIdentityUser CurrentUser => db.CurrentUser;
}