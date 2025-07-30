using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Blobs.Contracts.Queries;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.Posts.Domain;
using SocialMedia.SharedKernel;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Contracts;
using System.Linq.Expressions;

namespace SocialMedia.Posts.Application.UseCases.Queries;

internal sealed record GetPostsQuery(PostSearch Search) : Query<PagedResponse<PostDto>>;

internal class GetPostsQueryHandler(IPostDatabaseContext db, IMediator mediator) : EFQueryHandler<GetPostsQuery, PagedResponse<PostDto>>(db)
{
	public override async Task<Result<PagedResponse<PostDto>>> Handle(GetPostsQuery req, CancellationToken ct)
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

		var postIds = result.Items.Select(_ => _.Id).ToList();

		var blobIds = await db.Media
			.Where(_ => postIds.Contains(_.PostId))
			.OrderBy(_ => _.Order)
			.Select(_ => _.BlobId)
			.Distinct()
			.ToListAsync(ct);

		var blobsResult = await mediator.Send(new GetBlobsQuery(blobIds), ct);
		if (blobsResult.IsNotFound())
			return Result.NotFound(blobsResult.Errors.ToArray());

		var userIds = result.Items.Select(_ => _.UserId).Distinct().ToList();

		var usersResult = await mediator.Send(new GetUsersContractQuery(userIds), ct);
		if (usersResult.IsNotFound())
			return Result.NotFound(usersResult.Errors.ToArray());

		var blobMap = blobsResult.Value.ToDictionary(_ => _.Id);
		var userMap = usersResult.Value.ToDictionary(_ => _.Id);

		var likesMap = await db.PostLikes
			.Where(_ => postIds.Contains(_.PostId))
			.GroupBy(_ => _.PostId)
			.ToDictionaryAsync(_ => _.Key, _ => _.Count(), ct);

		var commentsMap = await db.Comments
			.Where(_ => postIds.Contains(_.PostId))
			.GroupBy(_ => _.PostId)
			.ToDictionaryAsync(_ => _.Key, _ => _.Count(), ct);

		foreach (var post in result.Items)
		{
			post.Media = blobIds
				.Where(blobMap.ContainsKey)
				.Select(_ => blobMap[_])
				.ToList();

			post.User = userMap.GetValueOrDefault(post.UserId);

			post.LikesCount = likesMap.GetValueOrDefault(post.Id);
			post.CommentsCount = commentsMap.GetValueOrDefault(post.Id);
		}

		return Result.Success(result);
	}
}