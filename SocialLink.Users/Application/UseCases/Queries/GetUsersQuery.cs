using SocialLink.Common.Application;
using SocialLink.Common.Data;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Domain;
using System.Linq.Expressions;

namespace SocialLink.Users.Application.UseCases.Queries;

internal sealed record GetUsersQuery(UserSearch Search) : Query<PagedResponse<UserDto>>;

internal class GetUsersQueryHandler(IUserDatabaseContext db) : EFQueryHandler<GetUsersQuery, PagedResponse<UserDto>>(db)
{
	public override async Task<ResponseWrapper<PagedResponse<UserDto>>> Handle(GetUsersQuery req, CancellationToken ct)
	{
		var search = req.Search;

		var filters = new List<Expression<Func<User, bool>>>();

		if (!string.IsNullOrWhiteSpace(search.Keyword))
			filters.Add(_ => _.Username.Contains(search.Keyword));

		if (search.Following)
			filters.Add(_ => _.Following.Any(_ => _.FollowerId == CurrentUser.Id));

		var result = await db.Users.EFSearchAsync(
			search,
			_ => _.Username,
			true,
			UserDto.Projection,
			filters,
			ct
		);

		return new(result);
	}
}