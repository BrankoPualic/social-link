using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application;

internal sealed record CommentSearch(Guid PostId) : PagedSearch;