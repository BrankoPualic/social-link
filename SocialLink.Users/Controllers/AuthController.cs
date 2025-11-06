using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Commands;

namespace SocialLink.Users.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
internal class AuthController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	[AllowAnonymous]
	public async Task<IActionResult> Login(LoginDto request, CancellationToken ct = default)
	{
		var result = await mediator.Send(new LoginCommand(request), ct);

		return result.IsSuccess
			? Ok(result.Data)
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
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}
}
