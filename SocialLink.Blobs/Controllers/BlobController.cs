using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Contracts.Queries;

namespace SocialLink.Blobs.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
internal class BlobController(IMediator mediator) : ControllerBase
{
	[HttpGet("{blobId}")]
	public async Task<IActionResult> Get([FromRoute] Guid blobId, CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetBlobQuery(blobId), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> GetList(List<Guid> blobIds, CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetBlobsQuery(blobIds), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> Restore(Guid blobId, CancellationToken ct = default)
	{
		var result = await mediator.Send(new RestoreBlobCommand(blobId), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> Delete(Guid blobId, CancellationToken ct = default)
	{
		var result = await mediator.Send(new DeleteBlobCommand(blobId), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}
}