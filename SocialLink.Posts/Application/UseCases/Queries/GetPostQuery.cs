using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialLink.Blobs.Contracts.Queries;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Contracts;

namespace SocialLink.Posts.Application.UseCases.Queries;
internal sealed record GetPostQuery(Guid PostId) : Query<PostDto>;

internal class GetPostQueryHandler(IPostDatabaseContext db, IMediator mediator) : EFQueryHandler<GetPostQuery, PostDto>(db)
{
	public override async Task<Result<PostDto>> Handle(GetPostQuery req, CancellationToken ct)
	{
		var postId = req.PostId;

		var model = await db.Posts
			.Select(PostDto.Projection)
			.FirstOrDefaultAsync(_ => _.Id == postId, ct);
		if (model is null)
			return Result.Invalid(new ValidationError(nameof(Post), "Post not found"));

		var blobIds = await db.Media
			.Where(_ => _.PostId == postId)
			.OrderBy(_ => _.Order)
			.Select(_ => _.BlobId)
			.ToListAsync(ct);

		var blobsResult = await mediator.Send(new GetBlobsQuery(blobIds), ct);
		if (blobsResult.IsNotFound())
			return Result.Invalid(new ValidationError(nameof(Post.Media), string.Join(',', blobsResult.Errors.ToArray())));

		var orderedBlobs = blobIds
			.Select(id => blobsResult.Value.FirstOrDefault(_ => _.Id == id))
			.Where(_ => _ is not null)
			.ToList();

		model.Media = orderedBlobs;

		var userResult = await mediator.Send(new GetUserContractQuery(model.UserId), ct);
		if (userResult.IsNotFound())
			return Result.Invalid(new ValidationError(nameof(PostDto.User), string.Join(',', userResult.Errors.ToArray())));

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

		model.User = userResult.Value;


		return Result.Success(model);
	}
}