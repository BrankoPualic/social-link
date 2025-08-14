using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Blobs.Contracts.Commands;

namespace SocialLink.Blobs.Endpoints;

[Authorize]
internal class DeleteBlob(IMediator mediator) : Endpoint<DeleteBlobRequest>
{
	public override void Configure()
	{
		Post("/blobs/delete");
	}

	public override async Task HandleAsync(DeleteBlobRequest req, CancellationToken ct)
	{
		var result = await mediator.Send(new DeleteBlobCommand(req.BlobId), ct);

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