using SocialLink.Blobs.Application.Interfaces;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Blobs.Contracts.Queries;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;

namespace SocialLink.Blobs.Integrations;

internal class GetBlobsQueryHandler(IBlobService blobService) : MongoQueryHandler<GetBlobsQuery, List<BlobDto>>
{
	public override async Task<ResponseWrapper<List<BlobDto>>> Handle(GetBlobsQuery req, CancellationToken ct)
	{
		var ids = req.BlobIds;
		if (ids.Count == 0)
			return new();

		var list = await Task.WhenAll(ids.Select(async id =>
		{
			var result = await blobService.GetBlobSasUriAsync(id);
			return result.IsSuccess ? new BlobDto { Id = id, Url = result.Data } : null;
		}));

		var data = list.Where(_ => _ != null).ToList();
		if (data.Count == 0)
			return new(new Error("Files not found."));

		return new(data);
	}
}