using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.Interfaces;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record AcceptFollowCommand(FollowDto Data) : Command;

internal class AcceptFollowCommandHandler(IUserDatabaseContext db, INotificationService notificationService) : EFCommandHandler<AcceptFollowCommand>(db)
{
	public override async Task<Result> Handle(AcceptFollowCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var follow = await db.Follows
			.Include(_ => _.Follower)
				.ThenInclude(_ => _.NotificationPreferences)
			.Include(_ => _.Following)
			.Where(_ => _.FollowerId == data.FollowerId)
			.Where(_ => _.FollowingId == data.FollowingId)
			.FirstOrDefaultAsync(ct);

		if (follow is null)
			return Result.NotFound("Follow request not found.");

		follow.IsPending = false;
		await db.SaveChangesAsync(false, ct);

		await notificationService.SendFollowAcceptedAsync(follow, ct);

		return Result.NoContent();
	}
}