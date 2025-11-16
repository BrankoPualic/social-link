using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialLink.Messaging.Application;
using SocialLink.Messaging.Application.Dtos;
using SocialLink.Messaging.Application.UseCases.Commands;
using SocialLink.Messaging.Application.UseCases.Queries;

namespace SocialLink.Messaging.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
internal class InboxController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> Get(InboxSearch search, CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetInboxQuery(search), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}

	[HttpGet("{chatGroupId}")]
	public async Task<IActionResult> GetConversation([FromRoute] Guid chatGroupId, CancellationToken ct = default)
	{
		var result = await mediator.Send(new GetConversationQuery(chatGroupId), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<IActionResult> CreateConversation(ConversationCreateDto data, CancellationToken ct = default)
	{
		var result = await mediator.Send(new CreateConversationCommand(data), ct);

		return result.IsSuccess
			? Ok(result.Data)
			: BadRequest(result.Errors);
	}
}