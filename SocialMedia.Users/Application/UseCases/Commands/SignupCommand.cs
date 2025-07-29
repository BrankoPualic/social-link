using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Blobs.Contracts.Commands;
using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Application.Interfaces;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Application.UseCases.Commands;

internal sealed record SignupCommand(SignupDto Data, FileInformationDto File) : Command<TokenDto>;

internal class SignupCommandHandler(IUserDatabaseContext db, IUserRepository userRepository, IAuthManager authManager, IMediator mediator) : EFCommandHandler<SignupCommand, TokenDto>(db)
{
	public override async Task<Result<TokenDto>> Handle(SignupCommand req, CancellationToken ct)
	{
		var data = req.Data;
		var file = req.File;

		var emailExists = await db.Users.AnyAsync(_ => _.Email == data.Email, ct);
		if (emailExists)
			return Result.Invalid(new ValidationError(nameof(User.Email), "Email is already in use."));

		var usernameExists = await db.Users.AnyAsync(_ => _.Username == data.Username, ct);
		if (usernameExists)
			return Result.Invalid(new ValidationError(nameof(User.Username), "Username is already in use."));

		User model = new();
		data.ToModel(model);
		model.Password = authManager.HashPassword(data.Password);
		db.Users.Add(model);

		if (file is not null)
		{
			var uploadResult = await mediator.Send(new UploadBlobCommand(file, SharedKernel.eBlobType.ProfileImage), ct);
			if (!uploadResult.IsSuccess)
				return Result.Invalid(new ValidationError(string.Join(',', uploadResult.Errors)));

			userRepository.CreateMedia(model.Id, uploadResult.Value);
		}

		// Log entry
		userRepository.CreateLoginLog(model.Id);

		await db.SaveChangesAsync(true, ct);

		return Result.Success(new TokenDto { Token = authManager.GenerateJwtToken(model) });
	}
}