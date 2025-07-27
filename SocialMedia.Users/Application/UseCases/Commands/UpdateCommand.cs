using Ardalis.Result;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Application.Dtos;

namespace SocialMedia.Users.Application.UseCases.Commands;

internal sealed record UpdateCommand(UserDto Data) : Command;

internal class UpdateCommandHandler(IUserDatabaseContext db) : EFCommandHandler<UpdateCommand>(db)
{
	public override async Task<Result> Handle(UpdateCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = await db.Users.FindAsync([data.Id], cancellationToken: ct);
		if (model is null)
			return Result.NotFound("User not found.");

		data.ToModel(model);

		await db.SaveChangesAsync(true, ct);

		return Result.NoContent();
	}
}