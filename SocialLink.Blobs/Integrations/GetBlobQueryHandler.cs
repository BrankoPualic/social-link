using Ardalis.Result;
using SocialLink.Blobs.Application.Interfaces;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Blobs.Contracts.Queries;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Blobs.Integrations;

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