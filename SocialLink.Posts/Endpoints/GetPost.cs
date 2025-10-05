using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Application.UseCases.Queries;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Endpoints;

[Authorize]
internal class GetPost(IMediator mediator) : Endpoint<GetPostRequest, PostDto>
{
	public override void Configure()
	{
		Get("/posts/{PostId}");
	}

	public override async Task HandleAsync(GetPostRequest req, CancellationToken ct)
	{
		var result = await mediator.Send(new GetPostQuery(req.PostId), ct);

		await result.SendResponseAsync(HttpContext, ct);
	}
}