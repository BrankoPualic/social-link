using SocialLink.Users.Application.Dtos;

namespace SocialLink.Users.Endpoints;

internal sealed record SaveNotificationPreferencesRequest(List<NotificationPreferenceDto> Preferences);