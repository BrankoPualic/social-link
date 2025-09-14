using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record UploadProfileImageCommand(Guid UserId, FileInformationDto File) : Command;

internal class UploadProfileImageCommandHandler(IUserDatabaseContext db, IUserRepository userRepository, IMediator mediator) : EFCommandHandler<UploadProfileImageCommand>(db)
{
	public override async Task<Result> Handle(UploadProfileImageCommand req, CancellationToken ct)
	{
		var userId = req.UserId;
		var file = req.File;

		if (userId == default || file is null)
			return Result.Invalid(new ValidationError("Parameters are empty"));

		var uploadResult = await mediator.Send(new UploadBlobCommand(new UploadFileDto(file, eBlobType.ProfileImage)), ct);
		if (!uploadResult.IsSuccess)
			return Result.Invalid(new ValidationError(string.Join(',', uploadResult.Errors)));

		var activeMedia = await db.Media
			.Where(_ => _.UserId == userId)
			.Where(_ => _.Type == eUserMedia.ProfileImage)
			.Where(_ => _.IsActive)
			.ToListAsync(ct);

		foreach (var media in activeMedia)
			media.IsActive = false;

		userRepository.CreateMedia(userId, uploadResult.Value.BlobId, eUserMedia.ProfileImage);

		await db.SaveChangesAsync(false, ct);

		await uploadResult.Value.Cleanup();

		await Task.WhenAll(activeMedia.Select(async _ =>
		{
			await mediator.Send(new DeleteBlobCommand(_.BlobId), ct);
		}));

		return Result.NoContent();
	}
}