using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Blobs.Contracts.Queries;

namespace SocialLink.Blobs.Endpoints;

[Authorize]
internal class GetBlobs(IMediator mediator) : Endpoint<GetBlobsRequest, List<BlobDto>>
{
	public override void Configure()
	{
		Post("/blobs");
	}

	public override async Task HandleAsync(GetBlobsRequest req, CancellationToken ct)
	{
		var result = await mediator.Send(new GetBlobsQuery(req.BlobIds), ct);

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