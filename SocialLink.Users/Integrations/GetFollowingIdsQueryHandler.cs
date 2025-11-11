using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.Users.Contracts;

namespace SocialLink.Users.Integrations;

internal class GetFollowingIdsQueryHandler(IUserDatabaseContext db) : EFQueryHandler<GetFollowingIdsQuery, List<Guid>>(db)
{
	public override async Task<ResponseWrapper<List<Guid>>> Handle(GetFollowingIdsQuery req, CancellationToken ct)
	{
		var result = await db.Follows
			.Where(_ => _.FollowerId == req.UserId)
			.Where(_ => !_.IsPending)
			.Select(_ => _.FollowingId)
			.ToListAsync(ct);

		return new(result);
	}
}