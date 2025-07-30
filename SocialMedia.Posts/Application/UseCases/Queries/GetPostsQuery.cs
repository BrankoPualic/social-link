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

		var postMedia = await db.Media
			.Where(_ => postIds.Contains(_.PostId))
			.OrderBy(_ => _.Order)
			.ToListAsync(ct);

		var blobIds = postMedia.Select(_ => _.BlobId).Distinct().ToList();

		var blobsResult = await mediator.Send(new GetBlobsQuery(blobIds), ct);
		if (blobsResult.IsNotFound())
			return Result.NotFound(blobsResult.Errors.ToArray());

		var userIds = result.Items.Select(_ => _.UserId).Distinct().ToList();

		var usersResult = await mediator.Send(new GetUsersContractQuery(userIds), ct);
		if (usersResult.IsNotFound())
			return Result.NotFound(usersResult.Errors.ToArray());

		var likesCount = await db.PostLikes
			.Where(_ => postIds.Contains(_.PostId))
			.GroupBy(_ => _.PostId)
			.Select(_ => new { PostId = _.Key, Count = _.Count() })
			.ToListAsync(ct);

		var commentsCount = await db.Comments
			.Where(_ => postIds.Contains(_.PostId))
			.GroupBy(_ => _.PostId)
			.Select(_ => new { PostId = _.Key, Count = _.Count() })
			.ToListAsync(ct);

		foreach (var post in result.Items)
		{
			var orderedBlobs = blobIds
				.SelectMany(id => blobsResult.Value.Where(_ => _.Id == id).ToList())
				.ToList();
			post.Media = orderedBlobs;

			post.User = usersResult.Value.FirstOrDefault(_ => _.Id == post.UserId);

			post.LikesCount = likesCount.FirstOrDefault(_ => _.PostId == post.Id)?.Count ?? 0;
			post.CommentsCount = commentsCount.FirstOrDefault(_ => _.PostId == post.Id)?.Count ?? 0;
		}

		return Result.Success(result);
	}
}