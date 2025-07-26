using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Notifications.Contracts;
using SocialMedia.Notifications.Contracts.Details;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Application.UseCases.Commands;

internal sealed record FollowCommand(FollowDto Data) : Command;

internal class FollowCommandHandler(IUserDatabaseContext db, IMediator mediator) : CommandHandler<FollowCommand>(db)
{
	public override async Task<Result> Handle(FollowCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var follower = await db.Users.FirstOrDefaultAsync(_ => _.Id == data.FollowerId, ct);
		if (follower is null)
			return Result.NotFound("Follower user not found.");

		var following = await db.Users.FirstOrDefaultAsync(_ => _.Id == data.FollowingId, ct);
		if (following is null)
			return Result.NotFound("Following user not found.");

		var follow = await db.Follows.FirstOrDefaultAsync(_ => _.FollowerId == data.FollowerId && _.FollowingId == data.FollowingId, ct);
		if (follow is not null)
		{
			string message = follow.IsPending
				? "Follow already sent."
				: "User already followed.";

			return Result.Invalid(new ValidationError(message));
		}

		var model = new UserFollow();
		data.ToModel(model);

		model.IsPending = following.IsPrivate;

		db.Follows.Add(model);
		await db.SaveChangesAsync(false, ct);

		var task = following.IsPrivate
			? mediator.Send(new CreateNotificationCommand(new FollowRequestDetails
			{
				UserId = data.FollowingId,
				FollowerId = data.FollowerId,
				FollowerName = follower.Username,
				Actions = [
					new () {
						Label = "Action",
						Endpoint = "/users/acceptFollow",
						Method = SharedKernel.eNotificationActionMethodType.Post
					},
					new () {
						Label = "Reject",
						Endpoint = "/users/rejectFollow",
						Method = SharedKernel.eNotificationActionMethodType.Post
					}
				]
			}), ct)
			: mediator.Send(new CreateNotificationCommand(new StartFollowingDetails
			{
				UserId = data.FollowingId,
				FollowerId = data.FollowerId,
				FollowerName = follower.Username,
			}), ct);

		await task;

		return Result.NoContent();
	}
}