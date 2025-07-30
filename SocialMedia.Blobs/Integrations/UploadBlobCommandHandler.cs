using Ardalis.Result;
using SocialMedia.Blobs.Application.Interfaces;
using SocialMedia.Blobs.Contracts.Commands;
using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Blobs.Integrations;

internal class UploadBlobCommandHandler(IBlobService blobService) : MongoCommandHandler<UploadBlobCommand, UploadResult>
{
	public override async Task<Result<UploadResult>> Handle(UploadBlobCommand req, CancellationToken ct)
	{
		var file = req.Data.File;
		var blobType = req.Data.BlobType;

		var (blob, cleanup) = await blobService.UploadAsync(file, null, blobType);

		return Result.Success(new UploadResult(blob.Id, cleanup));
	}
}