using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.Posts.Application.UseCases.Commands;

namespace SocialMedia.Posts.Endpoints;

[Authorize]
internal class UpdatePost(IMediator mediator) : Endpoint<PostEditDto>
{
	public override void Configure()
	{
		Post("/posts/update");
	}

	public override async Task HandleAsync(PostEditDto req, CancellationToken ct)
	{
		var result = await mediator.Send(new UpdatePostCommand(req), ct);

		if (result.IsNoContent())
		{
			await Send.NoContentAsync(ct);
		}
		else if (result.IsNotFound())
		{
			await Send.NotFoundAsync(ct);
		}
	}
}