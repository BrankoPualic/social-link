using SocialLink.Common.Application;

namespace SocialLink.Posts.Contracts;
public sealed record GetUserPostCountQuery(Guid UserId) : Query<int>;