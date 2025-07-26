using SocialMedia.SharedKernel;

namespace SocialMedia.Notifications.Application;
internal sealed record NotificationSearch(Guid UserId) : PagedSearch;