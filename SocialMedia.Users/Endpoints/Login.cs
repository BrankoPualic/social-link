using Ardalis.Result;
using FastEndpoints;
using MediatR;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Application.UseCases.Commands;

namespace SocialMedia.Users.Endpoints;

internal class Login(IMediator mediator) : Endpoint<LoginDto, TokenDto>
{
	public override void Configure()
	{
		Post("/api/users/login");
		AllowAnonymous();
	}

	public override async Task HandleAsync(LoginDto req, CancellationToken ct)
	{
		var result = await mediator.Send(new LoginCommand(req), ct);

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