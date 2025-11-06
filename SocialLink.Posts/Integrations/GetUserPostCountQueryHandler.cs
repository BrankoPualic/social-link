using Microsoft.EntityFrameworkCore;
using SocialLink.Posts.Contracts;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Posts.Integrations;

internal class GetUserPostCountQueryHandler(IPostDatabaseContext db) : EFQueryHandler<GetUserPostCountQuery, int>(db)
{
	public override async Task<ResponseWrapper<int>> Handle(GetUserPostCountQuery req, CancellationToken ct)
	{
		var userId = req.UserId;
		var count = await db.Posts
			.Where(_ => _.UserId == userId)
			.Where(_ => !_.IsArchived)
			.CountAsync(ct);
		return new(count);
	}
}