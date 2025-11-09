using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.Interfaces;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.UseCases.Commands;
internal sealed record LoginCommand(LoginDto Data) : Command;

internal class LoginCommandHandler(IUserDatabaseContext db, IUserRepository userRepository, IAuthManager authManager, IAuthTokenProcessor authTokenProcessor) : EFCommandHandler<LoginCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(LoginCommand req, CancellationToken ct)
	{
		var data = req.Data;

		var user = await userRepository.GetByEmailAsync(data.Email, ct);
		if (user is null)
			return new(new Error(nameof(User), "User not found."));

		bool passwordsMatch = authManager.VerifyPassword(data.Password, user.Password);
		if (!passwordsMatch)
			return new(new Error(nameof(User), "User not found."));

		// Log entry
		userRepository.CreateLoginLog(user.Id);

		// Generate Access and Refresh tokens
		var userRefreshToken = await userRepository.GetLatestRefreshTokenAsync(user.Id, ct) ?? new();

		var (jwtToken, expiresAt) = authTokenProcessor.GenerateJwtToken(user);
		var refreshToken = authTokenProcessor.GenerateRefreshToken();
		var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(Constants.REFRESH_TOKEN_DURATION_IN_DAYS);

		userRefreshToken.Token = refreshToken;
		userRefreshToken.TokenExpiresAt = refreshTokenExpiresAt;

		if (userRefreshToken.IsNew)
		{
			userRefreshToken.Id = Guid.NewGuid();
			userRefreshToken.UserId = user.Id;
			db.RefreshTokens.Add(userRefreshToken);
		}

		await db.SaveChangesAsync(true, ct);

		authTokenProcessor.WriteTokenAsHttpOnlyCookie(Constants.ACCESS_TOKEN_COOKIE, jwtToken, expiresAt);
		authTokenProcessor.WriteTokenAsHttpOnlyCookie(Constants.REFRESH_TOKEN_COOKIE, refreshToken, refreshTokenExpiresAt);

		return new();
	}
}