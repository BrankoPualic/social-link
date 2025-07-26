using SocialMedia.Notifications.Contracts.Details;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Notifications.Contracts;

public sealed record CreateNotificationCommand(NotificationDetails Data) : Command;