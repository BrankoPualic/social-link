using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialMedia.Posts.Application;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.Posts.Application.UseCases.Queries;
using SocialMedia.SharedKernel;

namespace SocialMedia.Posts.Endpoints;

[Authorize]
internal class GetComments(IMediator mediator) : Endpoint<CommentSearch, PagedResponse<CommentDto>>
{
	public override void Configure()
	{
		Post("/posts/comments");
	}

	public override async Task HandleAsync(CommentSearch req, CancellationToken ct)
	{
		var result = await mediator.Send(new GetCommentsQuery(req), ct);

		if (result.IsOk())
		{
			await Send.OkAsync(result, ct);
		}
		else if (result.IsNotFound())
		{
			await Send.NotFoundAsync(ct);
		}
	}
}