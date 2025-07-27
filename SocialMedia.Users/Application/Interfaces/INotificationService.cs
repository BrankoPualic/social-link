using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Application.Interfaces;

internal interface INotificationService
{
	Task SendFollowRequestAsync(User follower, User following, CancellationToken ct);

	Task SendFollowAcceptedAsync(UserFollow follow, CancellationToken ct);

	Task SendStartFollowingAsync(User follower, User following, CancellationToken ct);
}