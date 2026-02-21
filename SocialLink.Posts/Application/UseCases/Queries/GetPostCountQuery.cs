using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application.UseCases.Queries;

internal sealed record GetPostCountQuery : Query<int>, ICacheableQuery
{
	public string CacheKey => "postCount";

	public TimeSpan? CacheDuration => TimeSpan.FromMinutes(15);
}

internal class GetPostCountQueryHandler(IPostDatabaseContext db) : EFQueryHandler<GetPostCountQuery, int>(db)
{
	public override async Task<ResponseWrapper<int>> Handle(GetPostCountQuery req, CancellationToken ct)
	{
		return new(await db.Posts.CountAsync(ct));
	}
}
