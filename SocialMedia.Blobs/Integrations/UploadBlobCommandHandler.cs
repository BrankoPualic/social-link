using Ardalis.Result;
using SocialMedia.Blobs.Application.Interfaces;
using SocialMedia.Blobs.Contracts.Commands;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Blobs.Integrations;

internal class UploadBlobCommandHandler(IBlobService blobService) : MongoCommandHandler<UploadBlobCommand, Guid>
{
	public override async Task<Result<Guid>> Handle(UploadBlobCommand req, CancellationToken ct)
	{
		var file = req.File;
		var blobType = req.BlobType;

		var (blob, cleanup) = await blobService.UploadAsync(file, null, blobType);

		await cleanup();

		return Result.Success(blob.Id);
	}
}