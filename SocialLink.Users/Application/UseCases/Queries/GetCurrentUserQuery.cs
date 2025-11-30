using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;

namespace SocialLink.Users.Application.UseCases.Queries;

internal sealed record GetCurrentUserQuery(Guid UserId) : Query<CurrentUserDto>, ICacheableQuery
{
	public string CacheKey => $"currentUser:{UserId}";

	public TimeSpan? CacheDuration => TimeSpan.FromMinutes(20);
}

internal class GetCurrentUserQueryHandler(IUserDatabaseContext db) : EFQueryHandler<GetCurrentUserQuery, CurrentUserDto>(db)
{
	public override async Task<ResponseWrapper<CurrentUserDto>> Handle(GetCurrentUserQuery req, CancellationToken ct)
	{
		var result = await db.Users
			.Where(_ => _.Id == req.UserId)
			.Select(CurrentUserDto.Projection)
			.FirstOrDefaultAsync(ct);

		if (result is null)
			return new(new Error("User not found."));

		return new(result);
	}
}