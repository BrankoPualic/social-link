using SocialMedia.SharedKernel;

namespace SocialMedia.Users.Application;
internal sealed record UserSearch(string Keyword) : PagedSearch;