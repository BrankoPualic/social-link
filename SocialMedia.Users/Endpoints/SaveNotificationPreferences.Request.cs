using SocialMedia.Users.Application.Dtos;

namespace SocialMedia.Users.Endpoints;

internal class SaveNotificationPreferencesRequest
{
	public List<NotificationPreferenceDto> Preferences { get; set; } = [];
}