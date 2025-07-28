using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SocialMedia.Blobs.Application.Interfaces;
using SocialMedia.Blobs.Application.Services;
using SocialMedia.Blobs.Data;
using SocialMedia.Blobs.Data.Repositories;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace SocialMedia.Blobs;

public static class BlobsModule
{
	public static IServiceCollection AddBlobsModuleServices(this IServiceCollection services, IConfiguration config, ILogger logger, List<Assembly> mediatRAssemblies)
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

		services.AddScoped<IBlobDatabaseContext, BlobDatabaseContext>();

		services.AddScoped<IBlobService, BlobService>();
		services.AddScoped<IAzureBlobRepository, AzureBlobRepository>();

		mediatRAssemblies.Add(typeof(BlobsModule).Assembly);

		logger.Information("{Module} module services registered", "Blobs");

		return services;
	}
}