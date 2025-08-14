using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialLink.Users.Application.Dtos;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Users.Application.UseCases.Queries;
internal sealed record GetProfileQuery(Guid UserId) : Query<UserDto>;

internal class GetProfileQueryHandler(IUserDatabaseContext db) : EFQueryHandler<GetProfileQuery, UserDto>(db)
{
	public override async Task<Result<UserDto>> Handle(GetProfileQuery req, CancellationToken ct)
	{
		var userId = req.UserId;

		var result = await db.Users
			.Where(_ => _.Id == userId)
			.Select(UserDto.Projection)
			.FirstOrDefaultAsync(ct);

		if (result == null)
			return Result.NotFound("User not found.");

		return Result.Success(result);
	}
}