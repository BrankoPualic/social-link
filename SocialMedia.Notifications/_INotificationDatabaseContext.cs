using Microsoft.EntityFrameworkCore;
using SocialMedia.Notifications.Domain;
using SocialMedia.SharedKernel;

namespace SocialMedia.Notifications;

internal interface INotificationDatabaseContext : IAppDatabaseContext
{
	DbSet<Notification> Notifications { get; }
}