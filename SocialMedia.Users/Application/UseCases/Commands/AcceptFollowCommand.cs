using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Notifications.Contracts;
using SocialMedia.Notifications.Contracts.Details;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Application.Dtos;

namespace SocialMedia.Users.Application.UseCases.Commands;

internal sealed record AcceptFollowCommand(FollowDto Data) : Command;

internal class AcceptFollowCommandHandler(IUserDatabaseContext db, IMediator mediator) : EFCommandHandler<AcceptFollowCommand>(db)
{
	public override async Task<Result> Handle(AcceptFollowCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var follow = await db.Follows
			.Include(_ => _.Following)
			.Where(_ => _.FollowerId == data.FollowerId)
			.Where(_ => _.FollowingId == data.FollowingId)
			.FirstOrDefaultAsync(ct);

		if (follow is null)
			return Result.NotFound("Follow request not found.");

		follow.IsPending = false;
		await db.SaveChangesAsync(false, ct);

		await mediator.Send(new CreateNotificationCommand(new FollowAcceptedDetails
		{
			UserId = follow.FollowerId,
			FollowingId = follow.FollowingId,
			FollowingName = follow.Following.Username,
		}), ct);

		return Result.NoContent();
	}
}