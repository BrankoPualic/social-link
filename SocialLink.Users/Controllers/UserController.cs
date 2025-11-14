using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Users.Application;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Commands;
using SocialLink.Users.Application.UseCases.Queries;

namespace SocialLink.Users.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
internal class UserController(IMediator mediator) : ControllerBase
{
	[HttpGet("{userId}")]
	public async Task<IActionResult> Get([FromRoute] Guid userId, CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetUserQuery(userId), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> Search(UserSearch search, CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetUsersQuery(search), ct);
		return Ok(result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> SearchContracts(UserSearch search, CancellationToken ct = default)
	{
		var result = await mediator.Send(new SearchUserContractsQuery(search), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> Update(UserDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new UpdateUserCommand(request), ct);

		return result.IsSuccess
			? NoContent()
			: BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> UpdateProfileImage([FromForm] Guid userId, CancellationToken ct = default)
	{
		if (HttpContext.Request.Form.Files.Count is 0)
		{
			return BadRequest();
		}

		var fileReads = HttpContext.Request.Form.Files.Select(async file =>
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

		var fileDtos = await Task.WhenAll(fileReads);

		var result = await mediator.Send(new UpdateProfileImageCommand(userId, fileDtos.FirstOrDefault()), ct);

		return result.IsSuccess
			? NoContent()
			: BadRequest(result.Errors);
	}
}
