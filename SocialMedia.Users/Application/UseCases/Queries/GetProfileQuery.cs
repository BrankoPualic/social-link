using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Application.Dtos;

namespace SocialMedia.Users.Application.UseCases.Queries;
internal sealed record GetProfileQuery(Guid UserId) : Query<UserDto>;

internal class GetProfileQueryHandler(IDatabaseContext db) : QueryHandler<GetProfileQuery, UserDto>(db)
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