using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Application.Dtos;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record RejectFollowCommand(FollowDto Data) : Command;

internal class RejectFollowCommandHandler(IUserDatabaseContext db) : EFCommandHandler<RejectFollowCommand>(db)
{
	public override async Task<Result> Handle(RejectFollowCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var follow = await db.Follows
			.Where(_ => _.FollowerId == data.FollowerId)
			.Where(_ => _.FollowingId == data.FollowingId)
			.FirstOrDefaultAsync(ct);

		if (follow is null)
			return Result.Invalid(new ValidationError("Follow request not found"));

		db.Follows.Remove(follow);
		await db.SaveChangesAsync(false, ct);

		return Result.NoContent();
	}
}