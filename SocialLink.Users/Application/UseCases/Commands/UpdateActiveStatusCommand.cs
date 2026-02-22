using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record UpdateActiveStatusCommand(Guid UserId) : Command;

internal class UpdateActiveStatusCommandHandler(IUserDatabaseContext db) : EFCommandHandler<UpdateActiveStatusCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UpdateActiveStatusCommand req, CancellationToken ct)
	{
		var user = await db.Users.SingleOrDefaultAsync(_ => _.Id == req.UserId, ct);
		if (user is null)
			return new(new(nameof(User), "Not found"));

		user.IsActive = !user.IsActive;

		await db.SaveChangesAsync(true, ct);

		return new();
	}
}
