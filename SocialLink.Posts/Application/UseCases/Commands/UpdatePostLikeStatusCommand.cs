using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.Posts.Application.Dtos;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application.UseCases.Commands;

internal sealed record UpdatePostLikeStatusCommand(PostLikeDto Data) : Command;

internal class UpdatePostLikeStatusCommandHandler(IPostDatabaseContext db) : EFCommandHandler<UpdatePostLikeStatusCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UpdatePostLikeStatusCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = await db.PostLikes.FirstOrDefaultAsync(_ => _.PostId == data.PostId && _.UserId == data.UserId, ct);

		if (model is null)
		{
			db.PostLikes.Add(new()
			{
				PostId = data.PostId,
				UserId = data.UserId,
				LikedOn = DateTime.UtcNow,
			});
		}
		else
		{
			db.PostLikes.Remove(model);
		}

		await db.SaveChangesAsync(false, ct);

		return new();
	}
}