using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Commands;
using SocialLink.Users.Application.UseCases.Queries;

namespace SocialLink.Users.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
internal class FollowController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> Follow(FollowDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new FollowCommand(request), ct);

		return result.IsSuccess
			? NoContent()
			: BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> Unfollow(FollowDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new UnfollowCommand(request), ct);

		return result.IsSuccess
			? NoContent()
			: BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> CheckStatus(FollowDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new CheckFollowStatusQuery(request), ct);

		return Ok(result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Accept(FollowDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new AcceptFollowCommand(request), ct);

		return result.IsSuccess
			? NoContent()
			: BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> Reject(FollowDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new RejectFollowCommand(request), ct);

		return result.IsSuccess
			? NoContent()
			: BadRequest(result.Errors);
	}
}
