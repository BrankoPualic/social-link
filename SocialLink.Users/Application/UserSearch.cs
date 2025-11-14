using SocialLink.SharedKernel;

namespace SocialLink.Users.Application;

internal sealed record UserSearch : PagedSearch
{
	public string Keyword { get; set; }

	public bool Following { get; set; }
}