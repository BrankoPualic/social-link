using Ardalis.Result;
using SocialMedia.Blobs.Application.Interfaces;
using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.Blobs.Contracts.Queries;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Blobs.Integrations;

internal class GetBlobsQueryHandler(IBlobService blobService) : MongoQueryHandler<GetBlobsQuery, List<BlobDto>>
{
	public override async Task<Result<List<BlobDto>>> Handle(GetBlobsQuery req, CancellationToken ct)
	{
		var ids = req.BlobIds;
		if (ids.Count == 0)
			return Result.Success(new List<BlobDto>());

		var list = await Task.WhenAll(ids.Select(async id =>
		{
			var uriResult = await blobService.GetBlobSasUriAsync(id);
			return uriResult.IsSuccess ? new BlobDto { Id = id, Url = uriResult.Value } : null;
		}));

		var result = list.Where(_ => _ != null).ToList();
		if (result.Count == 0)
			return Result.NotFound("Files not found.");

		return Result.Success(result);
	}
}