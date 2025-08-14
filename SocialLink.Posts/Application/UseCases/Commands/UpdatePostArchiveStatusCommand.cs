using Ardalis.Result;
using SocialLink.Posts;
using SocialLink.Posts.Application.Dtos;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Posts.Application.UseCases.Commands;

internal sealed record UpdatePostArchiveStatusCommand(PostEditDto Data) : Command;

internal class UpdateArchivePostStatusCommandHandler(IPostDatabaseContext db) : EFCommandHandler<UpdatePostArchiveStatusCommand>(db)
{
	public override async Task<Result> Handle(UpdatePostArchiveStatusCommand req, CancellationToken ct)
	{
		var model = await db.Posts.FindAsync([req.Data.Id], ct);
		if (model is null)
			return Result.NotFound("Post not found");

		model.IsArchived = !model.IsArchived;

		await db.SaveChangesAsync(true, ct);

		return Result.NoContent();
	}
}