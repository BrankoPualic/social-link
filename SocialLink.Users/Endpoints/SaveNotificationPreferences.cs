using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SocialLink.Users.Application.UseCases.Commands;

namespace SocialLink.Users.Endpoints;

[Authorize]
internal class SaveNotificationPreferences(IMediator mediator) : Endpoint<SaveNotificationPreferencesRequest>
{
	public override void Configure()
	{
		Post("/users/saveNotificationPreferences");
	}

	public override async Task HandleAsync(SaveNotificationPreferencesRequest req, CancellationToken ct)
	{
		var result = await mediator.Send(new SaveNotificationPreferencesCommand(req.Preferences), ct);

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