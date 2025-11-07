using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application.UseCases.Commands;

internal sealed record UpdatePostCommand(PostEditDto Data) : Command;

internal class UpdatePostCommandHandler(IPostDatabaseContext db) : EFCommandHandler<UpdatePostCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UpdatePostCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = await db.Posts.FirstOrDefaultAsync(_ => _.Id == data.Id, ct);
		if (model is null)
			return new(new Error(nameof(Post), "Post not found."));

		data.ToModel(model);

		await db.SaveChangesAsync(true, ct);

		return new();
	}
}