using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;

namespace SocialLink.Users.Application.UseCases.Commands;

internal sealed record SaveNotificationPreferencesCommand(List<NotificationPreferenceDto> Preferences) : Command;

internal class SaveNotificationPreferencesCommandHandler(IUserDatabaseContext db) : EFCommandHandler<SaveNotificationPreferencesCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(SaveNotificationPreferencesCommand req, CancellationToken ct)
	{
		var preferences = req.Preferences;
		if (preferences.Count is 0)
			return new();

		var preferenceIds = preferences.Select(_ => _.Id).ToList();
		var userId = preferences.Select(_ => _.UserId).FirstOrDefault();
		if (userId == Guid.Empty)
			return new(new Error("User not found."));

		var existingPreferences = await db.NotificationPreferences
			.Where(_ => _.UserId == userId)
			.Where(_ => preferenceIds.Contains(_.Id))
			.ToListAsync(ct);

		foreach (var preference in preferences)
		{
			var model = existingPreferences.Where(_ => _.Id == preference.Id).FirstOrDefault() ?? new();
			preference.ToModel(model);

			if (model.IsNew)
			{
				model.Id = Guid.NewGuid();
				db.NotificationPreferences.Add(model);
			}
		}

		await db.SaveChangesAsync(false, ct);

		return new();
	}
}