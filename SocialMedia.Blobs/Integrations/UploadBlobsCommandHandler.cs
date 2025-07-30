using Ardalis.Result;
using SocialMedia.Blobs.Application.Interfaces;
using SocialMedia.Blobs.Contracts.Commands;
using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Blobs.Integrations;

internal class UploadBlobsCommandHandler(IBlobService blobService) : MongoCommandHandler<UploadBlobsCommand, List<UploadResult>>
{
	public override async Task<Result<List<UploadResult>>> Handle(UploadBlobsCommand req, CancellationToken ct)
	{
		var result = await Task.WhenAll(req.Data.Select(async _ =>
		{
			var (blob, cleanup) = await blobService.UploadAsync(_.File, null, _.BlobType);
			return new UploadResult(blob.Id, cleanup);
		}));

		return Result.Success(result.ToList());
	}
}