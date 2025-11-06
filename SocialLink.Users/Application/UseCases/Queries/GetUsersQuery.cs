using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;
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

		var filters = new List<Expression<Func<User, bool>>>
		{
			_ => _.IsActive,
			_ => !_.IsLocked,
			_ => !_.Roles.Select(_ => _.RoleId).Contains(eSystemRole.SystemAdministrator)
		};

		if (!string.IsNullOrWhiteSpace(search.Keyword))
			filters.Add(_ => _.Username.Contains(search.Keyword));

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