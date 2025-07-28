using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.Blobs.Contracts.Queries;

namespace SocialMedia.Blobs.Endpoints;

[Authorize]
internal class GetBlob(IMediator mediator) : Endpoint<GetBlobRequest, BlobDto>
{
	public override void Configure()
	{
		Get("/blobs/{BlobId}");
	}

	public override async Task HandleAsync(GetBlobRequest req, CancellationToken ct)
	{
		var result = await mediator.Send(new GetBlobQuery(req.BlobId), ct);

		if (result.IsSuccess)
		{
			await Send.OkAsync(result, ct);
		}
		else if (result.IsNotFound())
		{
			await Send.NotFoundAsync(ct);
		}
	}
}