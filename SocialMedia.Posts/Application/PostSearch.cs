using SocialMedia.SharedKernel;

namespace SocialMedia.Posts.Application;
internal sealed record PostSearch(Guid? UserId) : PagedSearch;