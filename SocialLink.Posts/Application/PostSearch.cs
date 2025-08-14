using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application;
internal sealed record PostSearch(Guid? UserId) : PagedSearch;