using SocialLink.Notifications.Contracts.Details;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Notifications.Contracts;

public sealed record CreateNotificationCommand(NotificationDetails Data) : Command;