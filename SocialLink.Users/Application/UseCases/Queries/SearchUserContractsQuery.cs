using MediatR;
using SocialLink.Common.Application;
using SocialLink.Common.Data;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Enumerators;
using SocialLink.Users.Contracts;
using SocialLink.Users.Domain;
using System.Linq.Expressions;

namespace SocialLink.Users.Application.UseCases.Queries;

internal sealed record SearchUserContractsQuery(UserSearch Search) : Query<PagedResponse<UserContractDto>>;

internal class SearchUserContractsQueryHandler(IUserDatabaseContext db, IMediator mediator) : EFQueryHandler<SearchUserContractsQuery, PagedResponse<UserContractDto>>(db)
{
	public override async Task<ResponseWrapper<PagedResponse<UserContractDto>>> Handle(SearchUserContractsQuery req, CancellationToken ct)
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

		if (search.Following)
			filters.Add(_ => _.Followers.Any(_ => _.FollowerId == CurrentUser.Id));

		var userIdsResponse = await db.Users.EFSearchAsync(
			search,
			_ => _.Username,
			true,
			_ => _.Id,
			filters,
			ct
		);

		var result = await mediator.Send(new GetUsersContractQuery(userIdsResponse.Items.ToList()), ct);
		if (!result.IsSuccess)
			return new(result.Errors);

		return new(new PagedResponse<UserContractDto>
		{
			TotalCount = userIdsResponse.TotalCount,
			CurrentPage = userIdsResponse.CurrentPage,
			PageSize = userIdsResponse.PageSize,
			Items = result.Data,
		});
	}
}