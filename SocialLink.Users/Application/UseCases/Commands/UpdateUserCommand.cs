using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record UpdateUserCommand(UserDto Data) : Command;

internal class UpdateUserCommandHandler(IUserDatabaseContext db, IUserRepository userRepository) : EFCommandHandler<UpdateUserCommand>(db)
{
	public override async Task<Result> Handle(UpdateUserCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = await db.Users
			.Where(_ => _.Id == data.Id)
			.FirstOrDefaultAsync(ct);

		if (model is null)
			return Result.Invalid(new ValidationError(nameof(User), "User not found"));

		if (data.Username != model.Username && await userRepository.IsUsernameTaken(data.Username, ct))
			return Result.Invalid(new ValidationError(nameof(User.Username), "Username is taken"));

		data.ToModel(model);

		await db.SaveChangesAsync(true, ct);

		return Result.NoContent();
	}
}