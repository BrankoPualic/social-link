using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.Posts.Application.UseCases.Queries;

namespace SocialMedia.Posts.Endpoints;

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

		if (result.IsSuccess)
		{
			await Send.OkAsync(result, ct);
		}
		else if (result.IsNotFound())
		{
			await Send.NotFoundAsync(ct);
		}
	}
}