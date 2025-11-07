using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.Posts.Application.Dtos;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application.UseCases.Commands;

internal sealed record UpdateCommentLikeStatusCommand(CommentLikeDto Data) : Command;

internal class UpdateCommentLikeStatusCommandHandler(IPostDatabaseContext db) : EFCommandHandler<UpdateCommentLikeStatusCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UpdateCommentLikeStatusCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = await db.CommentLikes.FirstOrDefaultAsync(_ => _.CommentId == data.CommentId && _.UserId == data.UserId, ct);

		if (model is null)
		{
			db.CommentLikes.Add(new()
			{
				CommentId = data.CommentId,
				UserId = data.UserId,
				LikedOn = DateTime.UtcNow,
			});
		}
		else
		{
			db.CommentLikes.Remove(model);
		}

		await db.SaveChangesAsync(false, ct);

		return new();
	}
}