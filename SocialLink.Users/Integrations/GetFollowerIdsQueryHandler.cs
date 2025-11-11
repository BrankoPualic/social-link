using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.Users.Contracts;

namespace SocialLink.Users.Integrations;

internal class GetFollowerIdsQueryHandler(IUserDatabaseContext db) : EFQueryHandler<GetFollowerIdsQuery, List<Guid>>(db)
{
	public override async Task<ResponseWrapper<List<Guid>>> Handle(GetFollowerIdsQuery req, CancellationToken ct)
	{
		var result = await db.Follows
			.Where(_ => _.FollowingId == req.UserId)
			.Where(_ => !_.IsPending)
			.Select(_ => _.FollowerId)
			.ToListAsync(ct);

		return new(result);
	}
}
