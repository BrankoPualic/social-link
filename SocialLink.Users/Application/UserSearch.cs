using SocialLink.SharedKernel;

namespace SocialLink.Users.Application;
internal sealed record UserSearch(string Keyword) : PagedSearch;