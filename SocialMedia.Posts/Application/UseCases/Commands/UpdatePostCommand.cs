using Ardalis.Result;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Posts.Application.UseCases.Commands;

internal sealed record UpdatePostCommand(PostEditDto Data) : Command;

internal class UpdatePostCommandHandler(IPostDatabaseContext db) : EFCommandHandler<UpdatePostCommand>(db)
{
	public override async Task<Result> Handle(UpdatePostCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = await db.Posts.FindAsync([data.Id], ct);
		if (model is null)
			return Result.NotFound("Post not found.");

		data.ToModel(model);

		await db.SaveChangesAsync(true, ct);

		return Result.NoContent();
	}
}