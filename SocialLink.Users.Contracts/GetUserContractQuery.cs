using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Users.Contracts;
public sealed record GetUserContractQuery(Guid UserId) : Query<UserContractDto>;