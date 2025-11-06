using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.Interfaces;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.UseCases.Commands;
internal sealed record LoginCommand(LoginDto Data) : Command<TokenDto>;

internal class LoginCommandHandler(IUserDatabaseContext db, IUserRepository userRepository, IAuthManager authManager) : EFCommandHandler<LoginCommand, TokenDto>(db)
{
	public override async Task<ResponseWrapper<TokenDto>> Handle(LoginCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = await userRepository.GetByEmailAsync(data.Email, ct);
		if (model is null)
			return new(new Error(nameof(User), "User not found.")); // TODO: Maybe put errors inside one UserErrors class??

		bool passwordsMatch = authManager.VerifyPassword(data.Password, model.Password);
		if (!passwordsMatch)
			return new(new Error(nameof(User), "User not found."));

		// Log entry
		userRepository.CreateLoginLog(model.Id);

		await db.SaveChangesAsync(true, ct);

		return new(new TokenDto { Content = authManager.GenerateJwtToken(model) });
	}
}