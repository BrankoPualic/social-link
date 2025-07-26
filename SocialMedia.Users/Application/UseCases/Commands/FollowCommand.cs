using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Application.UseCases.Commands;

internal sealed record FollowCommand(FollowDto Data) : Command;

internal class FollowCommandHandler(IUserDatabaseContext db) : CommandHandler<FollowCommand>(db)
{
	public override async Task<Result> Handle(FollowCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var followerExists = await db.Users.AnyAsync(_ => _.Id == data.FollowerId, ct);
		if (!followerExists)
			return Result.NotFound("Follower user not found.");

		var followingExists = await db.Users.AnyAsync(_ => _.Id == data.FollowingId, ct);
		if (!followingExists)
			return Result.NotFound("Following user not found.");

		var model = new UserFollow();
		data.ToModel(model);

		db.Follows.Add(model);
		await db.SaveChangesAsync(false, ct);

		return Result.NoContent();
	}
}