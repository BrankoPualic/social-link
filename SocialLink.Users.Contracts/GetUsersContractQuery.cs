using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Users.Contracts;

public sealed record GetUsersContractQuery(List<Guid> UserIds) : Query<List<UserContractDto>>;