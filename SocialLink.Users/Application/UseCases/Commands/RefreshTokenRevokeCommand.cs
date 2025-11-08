using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Interfaces;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record RefreshTokenRevokeCommand(string RefreshToken) : Command;

internal sealed class RefreshTokenRevokeCommandHandler(IUserDatabaseContext db, IAuthTokenProcessor authTokenProcessor) : EFCommandHandler<RefreshTokenRevokeCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(RefreshTokenRevokeCommand req, CancellationToken ct)
	{
		await db.RefreshTokens
			.Where(_ => _.Token == req.RefreshToken)
			.ExecuteDeleteAsync(ct);

		authTokenProcessor.DeleteCookie(Constants.ACCESS_TOKEN_COOKIE);
		authTokenProcessor.DeleteCookie(Constants.REFRESH_TOKEN_COOKIE);

		return new();
	}
}