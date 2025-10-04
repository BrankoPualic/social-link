using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Posts.Application;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Application.UseCases.Queries;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Endpoints;

[Authorize]
internal class GetPosts(IMediator mediator) : Endpoint<PostSearch, PagedResponse<PostDto>>
{
	public override void Configure()
	{
		Post("/posts");
	}

	public override async Task HandleAsync(PostSearch req, CancellationToken ct)
	{
		var result = await mediator.Send(new GetPostsQuery(req), ct);

		await result.SendResponseAsync(HttpContext, ct);
	}
}