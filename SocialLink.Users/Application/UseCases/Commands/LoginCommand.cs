using Ardalis.Result;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.Interfaces;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.UseCases.Commands;
internal sealed record LoginCommand(LoginDto Data) : Command<TokenDto>;

internal class LoginCommandHandler(IUserDatabaseContext db, IUserRepository userRepository, IAuthManager authManager) : EFCommandHandler<LoginCommand, TokenDto>(db)
{
	public override async Task<Result<TokenDto>> Handle(LoginCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var model = await userRepository.GetByEmailAsync(data.Email);
		if (model is null)
			return Result.Invalid(new ValidationError(nameof(User), "User not found"));

		bool passwordsMatch = authManager.VerifyPassword(data.Password, model.Password);
		if (!passwordsMatch)
			return Result.Invalid(new ValidationError(nameof(User), "User not found"));

		// Log entry
		userRepository.CreateLoginLog(model.Id);

		await db.SaveChangesAsync(true, ct);

		return Result.Success(new TokenDto { Content = authManager.GenerateJwtToken(model) });
	}
}