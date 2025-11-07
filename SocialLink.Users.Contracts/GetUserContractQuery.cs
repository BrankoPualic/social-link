using SocialLink.Common.Application;

namespace SocialLink.Users.Contracts;
public sealed record GetUserContractQuery(Guid UserId) : Query<UserContractDto>;