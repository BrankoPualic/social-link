using SocialMedia.SharedKernel;

namespace SocialMedia.Posts.Application;

internal sealed record CommentSearch(Guid PostId) : PagedSearch;