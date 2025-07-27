using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Application.Dtos;

namespace SocialMedia.Users.Application.UseCases.Commands;

internal sealed record SaveNotificationPreferencesCommand(List<NotificationPreferenceDto> Preferences) : Command;

internal class SaveNotificationPreferencesCommandHandler(IUserDatabaseContext db) : EFCommandHandler<SaveNotificationPreferencesCommand>(db)
{
	public override async Task<Result> Handle(SaveNotificationPreferencesCommand req, CancellationToken ct)
	{
		var preferences = req.Preferences;
		if (preferences.Count < 1)
			return Result.NoContent();

		var preferenceIds = preferences.Select(_ => _.Id).ToList();
		var userId = preferences.Select(_ => _.UserId).FirstOrDefault();
		if (userId == Guid.Empty)
			return Result.NotFound("User not found.");

		var existingPreferences = await db.NotificationPreferences
			.Where(_ => _.UserId == userId)
			.Where(_ => preferenceIds.Contains(_.Id))
			.ToListAsync(ct);

		foreach (var preference in preferences)
		{
			var model = existingPreferences.Where(_ => _.Id == preference.Id).FirstOrDefault() ?? new();
			preference.ToModel(model);

			if (model.IsNew)
				db.NotificationPreferences.Add(model);
		}

		await db.SaveChangesAsync(false, ct);

		return Result.NoContent();
	}
}