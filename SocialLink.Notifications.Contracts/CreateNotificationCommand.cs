using SocialLink.Common.Application;
using SocialLink.Notifications.Contracts.Details;

namespace SocialLink.Notifications.Contracts;

public sealed record CreateNotificationCommand(NotificationDetails Data) : Command;