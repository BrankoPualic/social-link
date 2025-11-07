using MediatR;
using SocialLink.Notifications.Contracts;
using SocialLink.Notifications.Contracts.Details;
using SocialLink.SharedKernel.Enumerators;
using SocialLink.Users.Application.Interfaces;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.Services;

internal class NotificationService(IMediator mediator) : INotificationService
{
	public async Task SendFollowRequestAsync(User follower, User following, CancellationToken ct)
	{
		if (following.IsPublic || following.IsMuted(eNotificationType.FollowRequest))
			return;

		await mediator.Send(new CreateNotificationCommand(new FollowRequestDetails
		{
			Message = FollowRequestDetails.DefaultMessage(follower.Username),
			UserId = following.Id,
			FollowerId = follower.Id,
			FollowerName = follower.Username,
			Actions = [
					new () {
						Label = "Accept",
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
			Message = FollowAcceptedDetails.DefaultMessage(follow.Following.Username),
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
			Message = StartFollowingDetails.DefaultMessage(follower.Username),
			UserId = following.Id,
			FollowerId = follower.Id,
			FollowerName = follower.Username,
		}), ct);
	}
}