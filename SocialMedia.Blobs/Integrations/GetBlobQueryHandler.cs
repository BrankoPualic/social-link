using Ardalis.Result;
using SocialMedia.Blobs.Application.Interfaces;
using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.Blobs.Contracts.Queries;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Blobs.Integrations;

internal class GetBlobQueryHandler(IBlobService blobService) : MongoQueryHandler<GetBlobQuery, BlobDto>
{
	public override async Task<Result<BlobDto>> Handle(GetBlobQuery req, CancellationToken ct)
	{
		var id = req.BlobId;
		var uriResult = await blobService.GetBlobSasUriAsync(id);
		if (uriResult.IsNotFound())
			return Result.NotFound(uriResult.Errors.ToArray());

		var result = new BlobDto
		{
			Id = id,
			Url = uriResult.Value
		};

		return Result.Success(result);
	}
}