using Ardalis.Result;
using SocialMedia.SharedKernel;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Domain;
using System.Linq.Expressions;

namespace SocialMedia.Users.Application.UseCases.Queries;

internal sealed record GetUsersQuery(UserSearch Search) : Query<PagedResponse<UserDto>>;

internal class GetUsersQueryHandler(IDatabaseContext db) : QueryHandler<GetUsersQuery, PagedResponse<UserDto>>(db)
{
	public override async Task<Result<PagedResponse<UserDto>>> Handle(GetUsersQuery req, CancellationToken ct)
	{
		var search = req.Search;

		var filters = new List<Expression<Func<User, bool>>>
		{
			_ => _.IsActive,
			_ => !_.IsLocked,
			_ => !_.Roles.Select(_ => _.RoleId).Contains(eSystemRole.SystemAdministrator)
		};

		if (!string.IsNullOrWhiteSpace(search.Keyword))
			filters.Add(_ => _.Username.Contains(search.Keyword));

		var result = await db.Users.SearchAsync(
			search,
			_ => _.Username,
			true,
			UserDto.Projection,
			filters,
			ct
		);

		return Result.Success(result);
	}
}