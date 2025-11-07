using SocialLink.Blobs.Application.Interfaces;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;

namespace SocialLink.Blobs.Integrations;

internal class UploadBlobCommandHandler(IBlobService blobService) : MongoCommandHandler<UploadBlobCommand, UploadResult>
{
	public override async Task<ResponseWrapper<UploadResult>> Handle(UploadBlobCommand req, CancellationToken ct)
	{
		var file = req.Data.File;
		var blobType = req.Data.BlobType;

		var (blob, cleanup) = await blobService.UploadAsync(file, null, blobType);

		return new(new UploadResult(blob.Id, cleanup));
	}
}