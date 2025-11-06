using Microsoft.EntityFrameworkCore;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record UpdateUserCommand(UserDto Data) : Command;

internal class UpdateUserCommandHandler(IUserDatabaseContext db, IUserRepository userRepository) : EFCommandHandler<UpdateUserCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UpdateUserCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = await db.Users
			.Where(_ => _.Id == data.Id)
			.FirstOrDefaultAsync(ct);

		if (model is null)
			return new(new Error(nameof(User), "User not found."));

		if (data.Username != model.Username && await userRepository.IsUsernameTaken(data.Username, ct))
			return new(new Error(nameof(User.Username), "Username is taken."));

		data.ToModel(model);

		await db.SaveChangesAsync(true, ct);

		return new();
	}
}