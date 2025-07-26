using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SocialMedia.Notifications.Data;
using System.Diagnostics;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace SocialMedia.Notifications;

public static class NotificationsModule
{
	public static IServiceCollection AddNotificationsModuleServices(this IServiceCollection services, IConfiguration config, ILogger logger, List<Assembly> mediatRAssemblies)
	{
		string connectionString = config.GetConnectionString("Database");
		services.AddDbContext<NotificationDatabaseContext>(options => options.UseSqlServer(connectionString, _ => _.CommandTimeout(600).EnableRetryOnFailure())
			.LogTo(_ => Debug.WriteLine(_), LogLevel.Warning));

		services.AddScoped<INotificationDatabaseContext, NotificationDatabaseContext>();

		mediatRAssemblies.Add(typeof(NotificationsModule).Assembly);

		logger.Information("{Module} module services registered", "Notifications");

		return services;
	}
}