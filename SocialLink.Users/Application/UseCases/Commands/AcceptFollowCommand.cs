using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.Interfaces;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record AcceptFollowCommand(FollowDto Data) : Command;

internal class AcceptFollowCommandHandler(IUserDatabaseContext db, INotificationService notificationService) : EFCommandHandler<AcceptFollowCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(AcceptFollowCommand req, CancellationToken ct)
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
			return new(new Error("Follow request not found."));

		follow.IsPending = false;
		await db.SaveChangesAsync(false, ct);

		await notificationService.SendFollowAcceptedAsync(follow, ct);

		return new();
	}
}