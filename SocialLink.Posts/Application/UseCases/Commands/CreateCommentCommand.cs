using Ardalis.Result;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Posts.Application.UseCases.Commands;

internal sealed record CreateCommentCommand(CommentEditDto Data) : Command<Guid>;

internal class CreateCommentCommandHandler(IPostDatabaseContext db) : EFCommandHandler<CreateCommentCommand, Guid>(db)
{
	public override async Task<Result<Guid>> Handle(CreateCommentCommand req, CancellationToken ct)
	{
		var data = req.Data;

		if (data is null || string.IsNullOrWhiteSpace(data?.Message))
			return Result.Invalid(new ValidationError(nameof(Comment), "Message is missing"));

		var model = new Comment();
		data.ToModel(model);

		db.Comments.Add(model);

		await db.SaveChangesAsync(true, ct);

		return Result.Created(model.Id);
	}
}