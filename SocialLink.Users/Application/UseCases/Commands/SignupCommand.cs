using MediatR;
using SocialLink.Blobs.Contracts;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Enumerators;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.Interfaces;
using SocialLink.Users.Domain;
using SocialLink.Users.Enumerators;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record SignupCommand(SignupDto Data, FileInformationDto File) : Command;

internal class SignupCommandHandler(IUserDatabaseContext db, IUserRepository userRepository, IAuthManager authManager, IAuthTokenProcessor authTokenProcessor, IMediator mediator) : EFCommandHandler<SignupCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(SignupCommand req, CancellationToken ct)
	{
		var data = req.Data;
		var file = req.File;

		if (await userRepository.IsEmailRegistered(data.Email, ct))
			return new(new Error(nameof(User.Email), "Email is already registered."));

		if (await userRepository.IsUsernameTaken(data.Username, ct))
			return new(new Error(nameof(User.Username), "Username is taken."));

		User model = new();
		data.ToModel(model);
		model.Password = authManager.HashPassword(data.Password);
		db.Users.Add(model);

		Cleanup cleanup = null;
		if (file is not null)
		{
			var uploadResult = await mediator.Send(new UploadBlobCommand(new UploadFileDto(file, eBlobType.ProfileImage)), ct);
			if (!uploadResult.IsSuccess)
				return new(uploadResult.Errors);

			cleanup = uploadResult.Data.Cleanup;
			userRepository.CreateMedia(model.Id, uploadResult.Data.BlobId, eUserMedia.ProfileImage);
		}

		// Log entry
		userRepository.CreateLoginLog(model.Id);

		// Generate Access and Refresh tokens
		var (jwtToken, expiresAt) = authTokenProcessor.GenerateJwtToken(model);
		var refreshToken = authTokenProcessor.GenerateRefreshToken();
		var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(Constants.REFRESH_TOKEN_DURATION_IN_DAYS);

		var userRefreshToken = new UserRefreshToken
		{
			Id = Guid.NewGuid(),
			UserId = model.Id,
			Token = refreshToken,
			TokenExpiresAt = refreshTokenExpiresAt,
		};

		db.RefreshTokens.Add(userRefreshToken);

		await db.SaveChangesAsync(true, ct);

		if (cleanup is not null)
			await cleanup.ExecuteAsync();

		authTokenProcessor.WriteTokenAsHttpOnlyCookie(Constants.ACCESS_TOKEN_COOKIE, jwtToken, expiresAt);
		authTokenProcessor.WriteTokenAsHttpOnlyCookie(Constants.REFRESH_TOKEN_COOKIE, refreshToken, refreshTokenExpiresAt);

		return new();
	}
}