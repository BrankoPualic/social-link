using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Users.Contracts;

public sealed record GetUsersContractQuery(List<Guid> UserIds) : Query<List<UserContractDto>>;