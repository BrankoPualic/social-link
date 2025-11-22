using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Messaging.Application;
using SocialLink.Messaging.Application.Dtos;
using SocialLink.Messaging.Application.UseCases.Commands;
using SocialLink.Messaging.Application.UseCases.Queries;

namespace SocialLink.Messaging.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
internal class MessageController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> Get(MessageSearch search, CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetMessagesQuery(search), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Create(MessageDto data, CancellationToken ct = default)
	{
		var result = await mediator.Send(new CreateMessageCommand(data), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> CreateAudioMessage([FromForm] Guid ChatGroupId, CancellationToken ct = default)
	{
		if (HttpContext.Request.Form.Files.Count is 0)
			return BadRequest();

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

		var result = await mediator.Send(new CreateAudioMessageCommand(ChatGroupId, fileDtos.FirstOrDefault()), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> ReadMessage(ReadMessageDto data, CancellationToken ct = default)
	{
		await mediator.Send(new ReadMessageCommand(data), ct);
		return NoContent();
	}
}