using SocialLink.SharedKernel;

namespace SocialLink.Messaging.Application;

public sealed record InboxSearch : PagedSearch
{
	public Guid UserId { get; set; }
}