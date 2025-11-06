using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Contracts;
using System.Linq.Expressions;

namespace SocialLink.Posts.Application.UseCases.Queries;

internal sealed record GetCommentsQuery(CommentSearch Search) : Query<PagedResponse<CommentDto>>;

internal class GetCommentsQueryHandler(IPostDatabaseContext db, IMediator mediator) : EFQueryHandler<GetCommentsQuery, PagedResponse<CommentDto>>(db)
{
	public override async Task<ResponseWrapper<PagedResponse<CommentDto>>> Handle(GetCommentsQuery req, CancellationToken ct)
	{
		var search = req.Search;
		if (search is null || search?.PostId is null)
			return new(new Error(nameof(Post), "Post not found."));

		var filters = new List<Expression<Func<Comment, bool>>>()
		{
			_ => _.PostId == search.PostId
		};

		var result = await db.Comments.EFSearchAsync(
			search,
			_ => _.CreatedOn,
			true,
			CommentDto.Projection,
			filters,
			ct
		);

		if (result.TotalCount == 0)
			return new();

		var userIds = result.Items.SelectIds(_ => _.UserId);

		var usersResult = await mediator.Send(new GetUsersContractQuery(userIds), ct);
		if (!usersResult.IsSuccess)
			return new(usersResult.Errors);

		var usersMap = usersResult.Data.ToDictionary(_ => _.Id);

		var commentIds = result.Items.SelectIds(_ => _.Id);

		var likesMap = await db.CommentLikes
			.Where(_ => commentIds.Contains(_.CommentId))
			.GroupBy(_ => _.CommentId)
			.ToDictionaryAsync(_ => _.Key, _ => new
			{
				Count = _.Count(),
				IsLiked = _.Any(_ => _.UserId == db.CurrentUser.Id)
			}, ct);

		var repliesMap = await db.Comments
			.Where(_ => commentIds.Contains(_.ParentId ?? Guid.Empty))
			.GroupBy(_ => _.ParentId)
			.ToDictionaryAsync(_ => _.Key, _ => _.Count(), ct);

		foreach (var comment in result.Items)
		{
			comment.User = usersMap.GetValueOrDefault(comment.UserId);
			comment.RepliesCount = repliesMap.GetValueOrDefault(comment.Id);

			comment.LikesCount = likesMap.GetValueOrDefault(comment.Id)?.Count;
			comment.IsLiked = likesMap.GetValueOrDefault(comment.Id)?.IsLiked;
		}

		return new(result);
	}
}