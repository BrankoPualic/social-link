using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Posts.Application;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Application.UseCases.Queries;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Endpoints;

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