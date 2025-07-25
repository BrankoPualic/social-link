using Ardalis.Result;
using FastEndpoints;
using MediatR;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Application.UseCases.Commands;

namespace SocialMedia.Users.Endpoints;

internal class Signup(IMediator mediator) : Endpoint<SignupDto, TokenDto>
{
	public override void Configure()
	{
		Post("/users/signup");
		AllowAnonymous();
	}

	public override async Task HandleAsync(SignupDto req, CancellationToken ct)
	{
		var result = await mediator.Send(new SignupCommand(req), ct);

		if (result.IsOk())
		{
			await Send.OkAsync(result, ct);
		}
		else if (result.IsInvalid())
		{
			await Send.ErrorsAsync(cancellation: ct);
		}
	}
}