using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record UnfollowCommand(FollowDto Data) : Command;

internal class UnfollowCommandHandler(IUserDatabaseContext db, IMemoryCache cache) : EFCommandHandler<UnfollowCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UnfollowCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var follow = await db.Follows
			.Where(_ => _.FollowerId == data.FollowerId)
			.Where(_ => _.FollowingId == data.FollowingId)
			.FirstOrDefaultAsync(ct);

		if (follow is null)
			return new(new Error("Follow doesn't exist."));

		db.Follows.Remove(follow);
		await db.SaveChangesAsync(false, ct);

		cache.Remove($"user:{data.FollowerId}");
		cache.Remove($"user:{data.FollowingId}");

		return new();
	}
}