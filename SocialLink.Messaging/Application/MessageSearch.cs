using SocialLink.SharedKernel;

namespace SocialLink.Messaging.Application;

public sealed record MessageSearch : PagedSearch
{
	public Guid ChatGroupId { get; set; }
}