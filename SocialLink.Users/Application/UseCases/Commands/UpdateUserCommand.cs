using Ardalis.Result;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Application.Dtos;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record UpdateUserCommand(UserDto Data) : Command;

internal class UpdateUserCommandHandler(IUserDatabaseContext db) : EFCommandHandler<UpdateUserCommand>(db)
{
	public override async Task<Result> Handle(UpdateUserCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = await db.Users.FindAsync([data.Id], cancellationToken: ct);
		if (model is null)
			return Result.Invalid(new ValidationError("User not found"));

		data.ToModel(model);

		await db.SaveChangesAsync(true, ct);

		return Result.NoContent();
	}
}