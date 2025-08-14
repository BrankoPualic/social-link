using Ardalis.Result;
using MediatR;
using SocialLink.Users.Domain;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record UploadProfileImageCommand(Guid UserId, FileInformationDto File) : Command;

internal class UploadProfileImageCommandHandler(IUserDatabaseContext db, IUserRepository userRepository, IMediator mediator) : EFCommandHandler<UploadProfileImageCommand>(db)
{
	public override async Task<Result> Handle(UploadProfileImageCommand req, CancellationToken ct)
	{
		var userId = req.UserId;
		var file = req.File;

		if (userId == default || file is null)
			return Result.Invalid(new ValidationError("Parameters are empty."));

		var uploadResult = await mediator.Send(new UploadBlobCommand(new UploadFileDto(file, SharedKernel.eBlobType.ProfileImage)), ct);
		if (!uploadResult.IsSuccess)
			return Result.Invalid(new ValidationError(string.Join(',', uploadResult.Errors)));

		userRepository.CreateMedia(userId, uploadResult.Value.BlobId);

		await db.SaveChangesAsync(false, ct);

		await uploadResult.Value.Cleanup();

		return Result.NoContent();
	}
}