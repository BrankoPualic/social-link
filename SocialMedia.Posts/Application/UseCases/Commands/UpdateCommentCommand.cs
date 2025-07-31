using Ardalis.Result;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Posts.Application.UseCases.Commands;

internal sealed record UpdateCommentCommand(CommentEditDto Data) : Command;

internal class UpdateCommentCommandHandler(IPostDatabaseContext db) : EFCommandHandler<UpdateCommentCommand>(db)
{
	public override async Task<Result> Handle(UpdateCommentCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = await db.Comments.FindAsync([data.Id], ct);
		if (model is null)
			return Result.NotFound("Comment not found.");

		data.ToModel(model);

		await db.SaveChangesAsync(true, ct);

		return Result.NoContent();
	}
}