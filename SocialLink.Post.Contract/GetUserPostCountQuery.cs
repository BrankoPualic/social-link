using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Posts.Contracts;
public sealed record GetUserPostCountQuery(Guid UserId) : Query<int>;