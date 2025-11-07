using SocialLink.Blobs.Application.Interfaces;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;

namespace SocialLink.Blobs.Integrations;

internal class UploadBlobsCommandHandler(IBlobService blobService, IFileValidationService fileValidationService) : MongoCommandHandler<UploadBlobsCommand, List<UploadResult>>
{
	public override async Task<ResponseWrapper<List<UploadResult>>> Handle(UploadBlobsCommand req, CancellationToken ct)
	{
		var validationResult = fileValidationService.Validate(req.Data.Select(_ => _.File).ToList());
		if (!validationResult.IsSuccess)
			return new(validationResult.Errors);

		var result = await Task.WhenAll(req.Data.Select(async _ =>
		{
			var (blob, cleanup) = (await blobService.UploadAsync(_.File, null, _.BlobType)).Data;
			return new UploadResult(blob.Id, cleanup);
		}));

		return new(result.ToList());
	}
}