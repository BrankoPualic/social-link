using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialLink.Users.Application.Dtos;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Users.Application.UseCases.Queries;

internal sealed record CheckFollowStatusQuery(FollowDto Data) : Query<bool>;

internal class CheckFollowStatusQueryHandler(IUserDatabaseContext db) : EFQueryHandler<CheckFollowStatusQuery, bool>(db)
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