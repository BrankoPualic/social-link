using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Posts.Application;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Application.UseCases.Commands;
using SocialLink.Posts.Application.UseCases.Queries;

namespace SocialLink.Posts.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
internal class PostController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Get([FromRoute] Guid postId, CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetPostQuery(postId), ct);

		if (result.IsSuccess)
			return Ok(result.Data);
		else
			return BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> GetList(PostSearch request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetPostsQuery(request), ct);

		if (result.IsSuccess)
			return Ok(result.Data);
		else
			return BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> Create(PostCreateDto request, CancellationToken ct = default)
	{
		var files = HttpContext.Request.Form.Files;

		var filesReads = files?.Select(async file =>
		{
			using var stream = new MemoryStream();
			await file.CopyToAsync(stream);
			return new FileInformationDto(
				file.FileName,
				file.ContentType,
				stream.ToArray(),
				file.Length
			);
		});

		var fileDtos = await Task.WhenAll(filesReads);

		var result = await mediator.Send(new CreatePostCommand(request, fileDtos.ToList()), ct);

		if (result.IsSuccess)
			return Ok(result.Data);
		else
			return BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> Update(PostEditDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new UpdatePostCommand(request), ct);

		if (result.IsSuccess)
			return NoContent();
		else
			return BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> UpdateArchiveStatus(PostEditDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new UpdatePostArchiveStatusCommand(request), ct);

		if (result.IsSuccess)
			return NoContent();
		else
			return BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> UpdateLikeStatus(PostLikeDto request, CancellationToken ct = default)
	{
		await mediator.Send(new UpdatePostLikeStatusCommand(request), ct);
		return NoContent();
	}
}