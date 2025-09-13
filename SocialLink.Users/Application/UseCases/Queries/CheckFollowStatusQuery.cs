using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Application.Dtos;

namespace SocialLink.Users.Application.UseCases.Queries;

internal sealed record CheckFollowStatusQuery(FollowDto Data) : Query<eFollowStatus>;

internal class CheckFollowStatusQueryHandler(IUserDatabaseContext db) : EFQueryHandler<CheckFollowStatusQuery, eFollowStatus>(db)
{
	public override async Task<Result<eFollowStatus>> Handle(CheckFollowStatusQuery req, CancellationToken ct)
	{
		var data = req.Data;

		var follow = await db.Follows
			.Where(_ => _.FollowerId == data.FollowerId)
			.Where(_ => _.FollowingId == data.FollowingId)
			.FirstOrDefaultAsync(ct);

		return Result.Success(
			follow is not null
				? follow.IsPending
				? eFollowStatus.Pending
				: eFollowStatus.Active
				: eFollowStatus.Unknown
		);
	}
}