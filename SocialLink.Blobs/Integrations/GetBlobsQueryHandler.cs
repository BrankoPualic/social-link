using Ardalis.Result;
using SocialLink.Blobs.Application.Interfaces;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Blobs.Contracts.Queries;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Blobs.Integrations;

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