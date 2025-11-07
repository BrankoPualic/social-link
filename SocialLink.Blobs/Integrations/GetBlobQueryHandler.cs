using SocialLink.Blobs.Application.Interfaces;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Blobs.Contracts.Queries;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;

namespace SocialLink.Blobs.Integrations;

internal class GetBlobQueryHandler(IBlobService blobService) : MongoQueryHandler<GetBlobQuery, BlobDto>
{
	public override async Task<ResponseWrapper<BlobDto>> Handle(GetBlobQuery req, CancellationToken ct)
	{
		var id = req.BlobId;
		var result = await blobService.GetBlobSasUriAsync(id);
		if (!result.IsSuccess)
			return new(result.Errors);

		var data = new BlobDto
		{
			Id = id,
			Url = result.Data
		};

		return new(data);
	}
}