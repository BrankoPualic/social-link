using Ardalis.Result;
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
	public override async Task<Result<PagedResponse<CommentDto>>> Handle(GetCommentsQuery req, CancellationToken ct)
	{
		var search = req.Search;
		if (search is null || search?.PostId is null)
			return Result.Invalid(new ValidationError(nameof(Post), "Post not found"));

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
			return Result.Success(new PagedResponse<CommentDto>());

		var userIds = result.Items.SelectIds(_ => _.UserId);

		var usersResult = await mediator.Send(new GetUsersContractQuery(userIds), ct);
		if (usersResult.IsNotFound())
			return Result.Invalid(new ValidationError(nameof(CommentDto.User), string.Join(',', usersResult.Errors.ToArray())));

		var usersMap = usersResult.Value.ToDictionary(_ => _.Id);

		var commentIds = result.Items.SelectIds(_ => _.Id);

		var likesMap = await db.CommentLikes
			.Where(_ => commentIds.Contains(_.CommentId))
			.GroupBy(_ => _.CommentId)
			.ToDictionaryAsync(_ => _.Key, _ => _.Count(), ct);

		var repliesMap = await db.Comments
			.Where(_ => commentIds.Contains(_.ParentId ?? Guid.Empty))
			.GroupBy(_ => _.ParentId)
			.ToDictionaryAsync(_ => _.Key, _ => _.Count(), ct);

		foreach (var comment in result.Items)
		{
			comment.User = usersMap.GetValueOrDefault(comment.UserId);
			comment.LikesCount = likesMap.GetValueOrDefault(comment.Id);
			comment.RepliesCount = repliesMap.GetValueOrDefault(comment.Id);
		}

		return Result.Success(result);
	}
}