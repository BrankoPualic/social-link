using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application.UseCases.Commands;

internal sealed record UpdateActiveStatusCommand(Guid PostId) : Command;

internal class UpdateActiveStatusCommandHandler(IPostDatabaseContext db) : EFCommandHandler<UpdateActiveStatusCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UpdateActiveStatusCommand req, CancellationToken ct)
	{
		var post = await db.Posts.SingleOrDefaultAsync(_ => _.Id == req.PostId, ct);
		if (post is null)
			return new(new(nameof(Post), "Not found"));

		post.IsActive = !post.IsActive;

		await db.SaveChangesAsync(true, ct);

		return new();
	}
}