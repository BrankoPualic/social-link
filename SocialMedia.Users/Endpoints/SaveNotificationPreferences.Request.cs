using SocialMedia.Users.Application.Dtos;

namespace SocialMedia.Users.Endpoints;

internal sealed record SaveNotificationPreferencesRequest(List<NotificationPreferenceDto> Preferences);