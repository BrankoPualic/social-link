using Ardalis.Result;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Application.Interfaces;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Application.UseCases.Commands;
internal sealed record LoginCommand(LoginDto Data) : Command<TokenDto>;

internal class LoginCommandHandler(IUserDatabaseContext db, IUserRepository userRepository, IAuthManager authManager) : CommandHandler<LoginCommand, TokenDto>(db)
{
	public override async Task<Result<TokenDto>> Handle(LoginCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = await userRepository.GetByEmailAsync(data.Email);
		if (model is null)
			return Result.NotFound("User not found.");

		bool passwordsMatch = authManager.VerifyPassword(data.Password, model.Password);
		if (!passwordsMatch)
			return Result.NotFound("User not found.");

		// Log entry
		userRepository.CreateLoginLog(model.Id);

		await db.SaveChangesAsync(true, ct);

		return Result.Success(new TokenDto { Token = authManager.GenerateJwtToken(model) });
	}
}