using Ardalis.Result;
using FastEndpoints;
using MediatR;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Application.UseCases.Commands;

namespace SocialLink.Users.Endpoints;

internal class Login(IMediator mediator) : Endpoint<LoginDto, TokenDto>
{
	public override void Configure()
	{
		Post("/users/login");
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