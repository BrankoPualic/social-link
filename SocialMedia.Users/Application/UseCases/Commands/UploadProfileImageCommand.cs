using Ardalis.Result;
using MediatR;
using SocialMedia.Blobs.Contracts.Commands;
using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Application.UseCases.Commands;

internal sealed record UploadProfileImageCommand(Guid UserId, FileInformationDto File) : Command;

internal class UploadProfileImageCommandHandler(IUserDatabaseContext db, IMediator mediator) : EFCommandHandler<UploadProfileImageCommand>(db)
{
	public override async Task<Result> Handle(UploadProfileImageCommand req, CancellationToken ct)
	{
		var userId = req.UserId;
		var file = req.File;

		if (userId == default || file is null)
			return Result.Invalid(new ValidationError("Parameters are empty."));

		var uploadResult = await mediator.Send(new UploadBlobCommand(file, SharedKernel.eBlobType.ProfileImage), ct);
		if (!uploadResult.IsSuccess)
			return Result.Invalid(new ValidationError(string.Join(',', uploadResult.Errors)));

		var model = new UserMedia
		{
			UserId = userId,
			BlobId = uploadResult.Value,
			IsActive = true,
			UploadedOn = DateTime.UtcNow
		};

		db.Media.Add(model);

		await db.SaveChangesAsync(false, ct);

		return Result.NoContent();
	}
}