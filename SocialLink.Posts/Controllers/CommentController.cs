using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialLink.Posts.Application;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Application.UseCases.Commands;
using SocialLink.Posts.Application.UseCases.Queries;

namespace SocialLink.Posts.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
internal class CommentController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> Get(CommentSearch request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetCommentsQuery(request), ct);

		if (result.IsSuccess)
			return Ok(result.Data);
		else
			return BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> Create(CommentEditDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new CreateCommentCommand(request), ct);

		if (result.IsSuccess)
			return Ok(result);
		else
			return BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> Update(CommentEditDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new UpdateCommentCommand(request), ct);

		if (result.IsSuccess)
			return NoContent();
		else
			return BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> UpdateLikeStatus(CommentLikeDto request, CancellationToken ct = default)
	{
		await mediator.Send(new UpdateCommentLikeStatusCommand(request), ct);
		return NoContent();
	}
}
