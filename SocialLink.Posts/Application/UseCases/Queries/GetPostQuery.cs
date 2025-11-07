using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialLink.Blobs.Contracts.Queries;
using SocialLink.Common.Application;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;
using SocialLink.Users.Contracts;

namespace SocialLink.Posts.Application.UseCases.Queries;
internal sealed record GetPostQuery(Guid PostId) : Query<PostDto>;

internal class GetPostQueryHandler(IPostDatabaseContext db, IMediator mediator) : EFQueryHandler<GetPostQuery, PostDto>(db)
{
	public override async Task<ResponseWrapper<PostDto>> Handle(GetPostQuery req, CancellationToken ct)
	{
		var postId = req.PostId;

		var model = await db.Posts
			.Select(PostDto.Projection)
			.FirstOrDefaultAsync(_ => _.Id == postId, ct);
		if (model is null)
			return new(new Error(nameof(Post), "Post not found."));

		var blobIds = await db.Media
			.Where(_ => _.PostId == postId)
			.OrderBy(_ => _.Order)
			.Select(_ => _.BlobId)
			.ToListAsync(ct);

		var blobsResult = await mediator.Send(new GetBlobsQuery(blobIds), ct);
		if (!blobsResult.IsSuccess)
			return new(blobsResult.Errors);

		var orderedBlobs = blobIds
			.Select(id => blobsResult.Data.FirstOrDefault(_ => _.Id == id))
			.Where(_ => _ is not null)
			.ToList();

		model.Media = orderedBlobs;

		var userResult = await mediator.Send(new GetUserContractQuery(model.UserId), ct);
		if (!userResult.IsSuccess)
			return new(userResult.Errors);

		var postLikeInfo = await db.PostLikes
			.Where(_ => _.PostId == postId)
			.GroupBy(_ => _.PostId)
			.Select(_ => new
			{
				LikesCount = _.Count(),
				IsLiked = _.Any(_ => _.UserId == db.CurrentUser.Id),
			})
			.FirstOrDefaultAsync(ct);

		model.LikesCount = postLikeInfo?.LikesCount;
		model.IsLiked = postLikeInfo?.IsLiked;
		model.CommentsCount = await db.Comments.CountAsync(_ => _.PostId == postId, ct);

		model.User = userResult.Data;

		return new(model);
	}
}