using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Interfaces;
using SocialLink.Users.Domain;
using SocialLink.Users.Exceptions;

namespace SocialLink.Users.Application.UseCases.Commands;
internal sealed record RefreshTokenCommand(string RefreshToken) : Command;

internal class RefreshTokenCommandHandler(IUserDatabaseContext db, IUserRepository userRepository, IAuthTokenProcessor authTokenProcessor) : EFCommandHandler<RefreshTokenCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(RefreshTokenCommand req, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(req.RefreshToken))
			throw new RefreshTokenException("Refresh token is missing.");

		var userRefreshToken = await userRepository.GetByRefreshTokenAsync(req.RefreshToken, ct);
		if (userRefreshToken is null)
			throw new RefreshTokenException("Unable to retrieve user for refresh token.");

		if (userRefreshToken.TokenExpiresAt < DateTime.UtcNow)
			throw new RefreshTokenException("Refresh token is expired.");

		// Generate Access and Refresh tokens
		var (jwtToken, expiresAt) = authTokenProcessor.GenerateJwtToken(userRefreshToken.User);
		var refreshToken = authTokenProcessor.GenerateRefreshToken();
		var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(Constants.REFRESH_TOKEN_DURATION_IN_DAYS);

		userRefreshToken.Token = refreshToken;
		userRefreshToken.TokenExpiresAt = refreshTokenExpiresAt;

		await db.SaveChangesAsync(true, ct);

		authTokenProcessor.WriteTokenAsHttpOnlyCookie(Constants.ACCESS_TOKEN_COOKIE, jwtToken, expiresAt);
		authTokenProcessor.WriteTokenAsHttpOnlyCookie(Constants.REFRESH_TOKEN_COOKIE, refreshToken, refreshTokenExpiresAt);

		return new();
	}
}
