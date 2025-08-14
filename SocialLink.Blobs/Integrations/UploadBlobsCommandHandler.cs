using Ardalis.Result;
using SocialLink.Blobs.Application.Interfaces;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Blobs.Integrations;

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