using SocialLink.Common.Application;

namespace SocialLink.Users.Contracts;

public sealed record GetFollowerIdsQuery(Guid UserId) : Query<List<Guid>>;
