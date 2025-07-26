using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Application.Dtos;

namespace SocialMedia.Users.Application.UseCases.Queries;

internal sealed record CheckFollowStatusQuery(FollowDto Data) : Query<bool>;

internal class CheckFollowStatusQueryHandler(IUserDatabaseContext db) : QueryHandler<CheckFollowStatusQuery, bool>(db)
{
	public override async Task<Result<bool>> Handle(CheckFollowStatusQuery req, CancellationToken ct)
	{
		var data = req.Data;

		var followExists = await db.Follows
			.AnyAsync(_ =>
				_.FollowerId == data.FollowerId &&
				_.FollowingId == data.FollowingId,
				ct
			);

		return Result.Success(followExists);
	}
}