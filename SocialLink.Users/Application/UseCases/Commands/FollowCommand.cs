using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.Interfaces;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record FollowCommand(FollowDto Data) : Command;

internal class FollowCommandHandler(IUserDatabaseContext db, INotificationService notificationService) : EFCommandHandler<FollowCommand>(db)
{
	public override async Task<Result> Handle(FollowCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var follower = await db.Users
			.Include(_ => _.NotificationPreferences)
			.AsNoTracking()
			.FirstOrDefaultAsync(_ => _.Id == data.FollowerId, ct);
		if (follower is null)
			return Result.Invalid(new ValidationError("Follower user not found"));

		var following = await db.Users
			.Include(_ => _.NotificationPreferences)
			.AsNoTracking()
			.FirstOrDefaultAsync(_ => _.Id == data.FollowingId, ct);
		if (following is null)
			return Result.Invalid(new ValidationError("Following user not found"));

		var follow = await db.Follows.FirstOrDefaultAsync(_ => _.FollowerId == data.FollowerId && _.FollowingId == data.FollowingId, ct);
		if (follow is not null)
		{
			string message = follow.IsPending
				? "Follow already sent"
				: "User already followed";

			return Result.Invalid(new ValidationError(message));
		}

		var model = new UserFollow();
		data.ToModel(model);

		model.IsPending = following.IsPrivate;

		db.Follows.Add(model);
		await db.SaveChangesAsync(false, ct);

		await notificationService.SendFollowRequestAsync(follower, following, ct);
		await notificationService.SendStartFollowingAsync(follower, following, ct);

		return Result.NoContent();
	}
}