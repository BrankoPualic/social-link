using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialLink.Blobs.Contracts.Queries;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Contracts;
using System.Linq.Expressions;

namespace SocialLink.Posts.Application.UseCases.Queries;

internal sealed record GetPostsQuery(PostSearch Search) : Query<PagedResponse<PostDto>>;

internal class GetPostsQueryHandler(IPostDatabaseContext db, IMediator mediator) : EFQueryHandler<GetPostsQuery, PagedResponse<PostDto>>(db)
{
	public override async Task<ResponseWrapper<PagedResponse<PostDto>>> Handle(GetPostsQuery req, CancellationToken ct)
	{
		var search = req.Search;

		var filters = new List<Expression<Func<Post, bool>>>();

		if (search.UserId.HasValue)
			filters.Add(_ => _.UserId == search.UserId);

		var result = await db.Posts.EFSearchAsync(
			search,
			_ => _.CreatedOn,
			true,
			PostDto.Projection,
			filters,
			ct
		);

		if (result.TotalCount == 0)
			return new();

		var postIds = result.Items.SelectIds(_ => _.Id);

		var blobIds = await db.Media
			.Where(_ => postIds.Contains(_.PostId))
			.OrderBy(_ => _.Order)
			.Select(_ => _.BlobId)
			.Distinct()
			.ToListAsync(ct);

		var blobsResult = await mediator.Send(new GetBlobsQuery(blobIds), ct);
		if (!blobsResult.IsSuccess)
			return new(blobsResult.Errors);

		var userIds = result.Items.SelectIds(_ => _.UserId);

		var usersResult = await mediator.Send(new GetUsersContractQuery(userIds), ct);
		if (!usersResult.IsSuccess)
			return new(usersResult.Errors);

		var blobsMap = blobsResult.Data.ToDictionary(_ => _.Id);
		var usersMap = usersResult.Data.ToDictionary(_ => _.Id);

		var likesMap = await db.PostLikes
			.Where(_ => postIds.Contains(_.PostId))
			.GroupBy(_ => _.PostId)
			.ToDictionaryAsync(_ => _.Key, _ => new
			{
				Count = _.Count(),
				IsLiked = _.Any(_ => _.UserId == db.CurrentUser.Id)
			}, ct);

		var commentsMap = await db.Comments
			.Where(_ => postIds.Contains(_.PostId))
			.GroupBy(_ => _.PostId)
			.ToDictionaryAsync(_ => _.Key, _ => _.Count(), ct);

		foreach (var post in result.Items)
		{
			post.Media = blobIds
				.Where(blobsMap.ContainsKey)
				.Select(_ => blobsMap[_])
				.ToList();

			post.User = usersMap.GetValueOrDefault(post.UserId);

			post.LikesCount = likesMap.GetValueOrDefault(post.Id)?.Count;
			post.CommentsCount = commentsMap.GetValueOrDefault(post.Id);

			post.IsLiked = likesMap.GetValueOrDefault(post.Id)?.IsLiked;
		}

		return new(result);
	}
}