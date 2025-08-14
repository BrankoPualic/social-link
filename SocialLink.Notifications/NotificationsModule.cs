using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SocialLink.Notifications.Application.ScheduledTasks;
using SocialLink.Notifications.Data;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace SocialLink.Notifications;

public static class NotificationsModule
{
	public static IServiceCollection AddNotificationsModuleServices(this IServiceCollection services, IConfiguration config, ILogger logger, List<Assembly> mediatRAssemblies)
	{
		// MongoDb
		string connectionString = config["MongoDBConnectionString"];
		services.AddSingleton<IMongoClient>(_ =>
		{
			var settings = MongoClientSettings.FromConnectionString(connectionString);

			// Set the ServerApi field of the settings object to set the version of the Stable API on the client
			settings.ServerApi = new ServerApi(ServerApiVersion.V1);

			// Create a new client and connect to the server
			var client = new MongoClient(settings);
			return client;
		});
		services.AddSingleton<IMongoHealthChecker, MongoHealthChecker>();

		services.AddScoped<INotificationMongoContext, NotificationMongoContext>();

		services.AddHostedService<OldNotificationsCleanupScheduleTask>();
		services.AddHostedService<ReadNotificationsCleanupScheduleTask>();

		mediatRAssemblies.Add(typeof(NotificationsModule).Assembly);

		logger.Information("{Module} module services registered", "Notifications");

		return services;
	}
}