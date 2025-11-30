using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Enumerators;
using SocialLink.Users.Domain;
using SocialLink.Users.Enumerators;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record UpdateProfileImageCommand(Guid UserId, FileInformationDto File) : Command;

internal class UpdateProfileImageCommandHandler(IUserDatabaseContext db, IUserRepository userRepository, IMediator mediator, IMemoryCache cache) : EFCommandHandler<UpdateProfileImageCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UpdateProfileImageCommand req, CancellationToken ct)
	{
		var userId = req.UserId;
		var file = req.File;

		if (userId == default || file is null)
			return new(new Error("Parameters are empty."));

		var uploadResult = await mediator.Send(new UploadBlobCommand(new UploadFileDto(file, eBlobType.ProfileImage)), ct);
		if (!uploadResult.IsSuccess)
			return new(uploadResult.Errors);

		var activeMedia = await db.Media
			.Where(_ => _.UserId == userId)
			.Where(_ => _.Type == eUserMedia.ProfileImage)
			.Where(_ => _.IsActive)
			.ToListAsync(ct);

		foreach (var media in activeMedia)
			media.IsActive = false;

		userRepository.CreateMedia(userId, uploadResult.Data.BlobId, eUserMedia.ProfileImage);

		await db.SaveChangesAsync(false, ct);

		await uploadResult.Data.Cleanup.ExecuteAsync();

		await Task.WhenAll(activeMedia.Select(async _ =>
		{
			await mediator.Send(new DeleteBlobCommand(_.BlobId), ct);
		}));

		cache.Remove($"user:{userId}");

		return new();
	}
}