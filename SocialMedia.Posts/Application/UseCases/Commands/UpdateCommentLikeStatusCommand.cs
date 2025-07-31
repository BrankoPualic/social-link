﻿using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Posts.Application.UseCases.Commands;

internal sealed record UpdateCommentLikeStatusCommand(CommentLikeDto Data) : Command;

internal class UpdateCommentLikeStatusCommandHandler(IPostDatabaseContext db) : EFCommandHandler<UpdateCommentLikeStatusCommand>(db)
{
	public override async Task<Result> Handle(UpdateCommentLikeStatusCommand req, CancellationToken ct)
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

		return Result.NoContent();
	}
}