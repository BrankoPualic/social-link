using SocialLink.SharedKernel;

namespace SocialLink.Notifications.Application;
internal sealed record NotificationSearch(Guid UserId) : PagedSearch;