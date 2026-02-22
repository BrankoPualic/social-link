using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record UpdateLockStatusCommand(Guid UserId) : Command;

internal class UpdateLockStatusCommandHandler(IUserDatabaseContext db) : EFCommandHandler<UpdateLockStatusCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UpdateLockStatusCommand req, CancellationToken ct)
	{
		var user = await db.Users.SingleOrDefaultAsync(_ => _.Id == req.UserId, ct);
		if (user is null)
			return new(new(nameof(User), "Not found"));

		user.IsLocked = !user.IsLocked;

		await db.SaveChangesAsync(true, ct);

		return new();
	}
}
