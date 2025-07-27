using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace SocialMedia.Notifications;

public interface IMongoHealthChecker
{
	Task CheckAsync(IConfiguration config, ILogger logger);
}

public class MongoHealthChecker(IMongoClient client) : IMongoHealthChecker
{
	public async Task CheckAsync(IConfiguration config, ILogger logger)
	{
		try
		{
			var database = config.GetSection("MongoDatabase")["Name"];
			await client.GetDatabase(database).RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
			logger.Information("Pinged your deployment. You Successfully connected to {Database} MongoDB!", database);
		}
		catch (Exception ex)
		{
			logger.Error(ex, ex.Message);
		}
	}
}