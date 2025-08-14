using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Blobs.Contracts.Commands;

namespace SocialLink.Blobs.Endpoints;

[Authorize]
internal class RestoreBlob(IMediator mediator) : Endpoint<RestoreBlobRequest>
{
	public override void Configure()
	{
		Post("/blobs/restore");
	}

	public override async Task HandleAsync(RestoreBlobRequest req, CancellationToken ct)
	{
		var result = await mediator.Send(new RestoreBlobCommand(req.BlobId), ct);

		if (result.IsSuccess)
		{
			await Send.NoContentAsync(ct);
		}
		else if (result.IsNotFound())
		{
			await Send.NotFoundAsync(ct);
		}
	}
}