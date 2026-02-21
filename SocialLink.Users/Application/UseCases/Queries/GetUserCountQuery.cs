using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;

namespace SocialLink.Users.Application.UseCases.Queries;

internal sealed record GetUserCountQuery(UserCountSearch Search) : Query<int>, ICacheableQuery
{
	public string CacheKey => "userCount";

	public TimeSpan? CacheDuration => TimeSpan.FromMinutes(15);
}

internal class GetUserCountQueryHandler(IUserDatabaseContext db) : EFQueryHandler<GetUserCountQuery, int>(db)
{
	public override async Task<ResponseWrapper<int>> Handle(GetUserCountQuery req, CancellationToken ct)
	{
		var search = req.Search;

		var query = db.Users;

		if (search.IsLocked)
			query.Where(_ => _.IsLocked);

		if (search.IsActive)
			query.Where(_ => _.IsActive);

		if (search.GenderId.HasValue)
			query.Where(_ => _.GenderId == search.GenderId.Value);

		return new(await query.CountAsync(ct));
	}
}
