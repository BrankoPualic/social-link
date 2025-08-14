using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Blobs.Contracts.Queries;
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
			return Result.NotFound("Post not found.");

		var blobIds = await db.Media
			.Where(_ => _.PostId == postId)
			.OrderBy(_ => _.Order)
			.Select(_ => _.BlobId)
			.ToListAsync(ct);

		var blobsResult = await mediator.Send(new GetBlobsQuery(blobIds), ct);
		if (blobsResult.IsNotFound())
			return Result.NotFound(blobsResult.Errors.ToArray());

		var orderedBlobs = blobIds
			.Select(id => blobsResult.Value.FirstOrDefault(_ => _.Id == id))
			.Where(_ => _ is not null)
			.ToList();

		model.Media = orderedBlobs;

		var userResult = await mediator.Send(new GetUserContractQuery(model.UserId), ct);
		if (userResult.IsNotFound())
			return Result.NotFound(userResult.Errors.ToArray());

		model.LikesCount = await db.PostLikes.CountAsync(_ => _.PostId == postId, ct);
		model.CommentsCount = await db.Comments.CountAsync(_ => _.PostId == postId, ct);

		model.User = userResult.Value;

		return Result.Success(model);
	}
}