using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SocialLink.Messaging.Application.UseCases.Commands;
using SocialLink.Messaging.Data;
using SocialLink.Messaging.Domain;
using SocialLink.Messaging.Hubs.Presence;
using System.Diagnostics;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace SocialLink.Messaging;

public static class MessagingModule
{
	public static IServiceCollection AddMessagingModuleServices(this IServiceCollection services, IConfiguration config, ILogger logger, List<Assembly> mediatRAssemblies)
	{
		services.AddSignalR();

		// MongoDb
		string mongoConnectionString = config["MongoDbConnectionString"];
		services.AddSingleton<IMongoClient>(_ =>
		{
			var settings = MongoClientSettings.FromConnectionString(mongoConnectionString);

			// Set the ServerApi field of the settings object to set the version of the Stable API on the client
			settings.ServerApi = new ServerApi(ServerApiVersion.V1);

			// Create a new client and connect to the server
			var client = new MongoClient(settings);
			return client;
		});

		string efConnectionString = config.GetConnectionString("Database");
		services.AddDbContext<EFMessagingDatabaseContext>(options => options.UseSqlServer(efConnectionString, _ => _.CommandTimeout(600).EnableRetryOnFailure())
			.LogTo(_ => Debug.WriteLine(_), LogLevel.Warning));

		services.AddControllers()
			.PartManager.ApplicationParts.Add(new AssemblyPart(typeof(MessagingModule).Assembly));

		services.AddScoped<IEFMessagingDatabaseContext, EFMessagingDatabaseContext>();
		services.AddScoped<IMongoMessagingDatabaseContext, MongoMessagingDatabaseContext>();

		services.AddScoped<IChatGroupRepository, ChatGroupRepository>();

		services.AddSingleton<IPresenceTracker, PresenceTracker>();

		services.AddTransient<IValidator<CreateMessageCommand>, CreateMessageCommandValidator>();
		services.AddTransient<IValidator<CreateAudioMessageCommand>, CreateAudioMessageCommandValidator>();

		mediatRAssemblies.Add(typeof(MessagingModule).Assembly);

		logger.Information("{Module} module services registered", "Messaging");

		return services;
	}
}