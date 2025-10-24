using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Commands;
using SocialLink.Users.Application.UseCases.Queries;

namespace SocialLink.Users.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
internal class AuthController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	[Authorize]
	public async Task<IActionResult> GetCurrentUser(CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetCurrentUserQuery(), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<IActionResult> Login(LoginDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new LoginCommand(request), ct);

		return result.IsSuccess
			? NoContent()
			: BadRequest(result.Errors);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<IActionResult> Signup(SignupDto request, CancellationToken ct = default)
	{
		var fileReads = HttpContext.Request.Form.Files?.Select(async file =>
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

		var result = await mediator.Send(new SignupCommand(request, fileDtos.FirstOrDefault()), ct);

		return result.IsSuccess
			? NoContent()
			: BadRequest(result.Errors);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<IActionResult> RefreshToken(CancellationToken ct = default)
	{
		var refreshToken = HttpContext.Request.Cookies[Constants.REFRESH_TOKEN_COOKIE];

		_ = await mediator.Send(new RefreshTokenCommand(refreshToken), ct);

		return NoContent();
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<IActionResult> Logout(CancellationToken ct = default)
	{
		var refreshToken = HttpContext.Request.Cookies[Constants.REFRESH_TOKEN_COOKIE];

		_ = await mediator.Send(new RefreshTokenRevokeCommand(refreshToken), ct);

		return NoContent();
	}
}
