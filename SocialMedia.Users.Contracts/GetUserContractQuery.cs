using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Users.Contracts;
public sealed record GetUserContractQuery(Guid UserId) : Query<UserContractDto>;