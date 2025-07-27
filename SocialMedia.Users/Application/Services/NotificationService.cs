using MediatR;
using SocialMedia.Notifications.Contracts;
using SocialMedia.Notifications.Contracts.Details;
using SocialMedia.SharedKernel;
using SocialMedia.Users.Application.Interfaces;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Application.Services;

internal class NotificationService(IMediator mediator) : INotificationService
{
	public async Task SendFollowRequestAsync(User follower, User following, CancellationToken ct)
	{
		if (following.IsPublic || following.IsMuted(eNotificationType.FollowRequest))
			return;

		await mediator.Send(new CreateNotificationCommand(new FollowRequestDetails
		{
			UserId = following.Id,
			FollowerId = follower.Id,
			FollowerName = follower.Username,
			Actions = [
					new () {
						Label = "Action",
						Endpoint = "/users/acceptFollow",
						Method = eNotificationActionMethodType.Post
					},
					new () {
						Label = "Reject",
						Endpoint = "/users/rejectFollow",
						Method = eNotificationActionMethodType.Post
					}
				]
		}), ct);
	}

	public async Task SendFollowAcceptedAsync(UserFollow follow, CancellationToken ct)
	{
		if (follow.Follower.IsMuted(eNotificationType.FollowAccepted))
			return;

		await mediator.Send(new CreateNotificationCommand(new FollowAcceptedDetails
		{
			UserId = follow.FollowerId,
			FollowingId = follow.FollowingId,
			FollowingName = follow.Following.Username,
		}), ct);
	}

	public async Task SendStartFollowingAsync(User follower, User following, CancellationToken ct)
	{
		if (following.IsPrivate || following.IsMuted(eNotificationType.StartFollowing))
			return;

		await mediator.Send(new CreateNotificationCommand(new StartFollowingDetails
		{
			UserId = following.Id,
			FollowerId = follower.Id,
			FollowerName = follower.Username,
		}), ct);
	}
}