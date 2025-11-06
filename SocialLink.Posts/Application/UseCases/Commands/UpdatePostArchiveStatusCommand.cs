using SocialLink.Posts.Application.Dtos;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Posts.Application.UseCases.Commands;

internal sealed record UpdatePostArchiveStatusCommand(PostEditDto Data) : Command;

internal class UpdateArchivePostStatusCommandHandler(IPostDatabaseContext db) : EFCommandHandler<UpdatePostArchiveStatusCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UpdatePostArchiveStatusCommand req, CancellationToken ct)
	{
		var model = await db.Posts.FindAsync([req.Data.Id], ct);
		if (model is null)
			return new(new Error("Post not found."));

		model.IsArchived = !model.IsArchived;

		await db.SaveChangesAsync(true, ct);

		return new();
	}
}