using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SocialLink.Posts.Data;
using System.Diagnostics;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace SocialLink.Posts;

public static class PostsModule
{
	public static IServiceCollection AddPostsModuleServices(this IServiceCollection services, IConfiguration config, ILogger logger, List<Assembly> mediatRAssemblies)
	{
		string connectionString = config.GetConnectionString("Database");
		services.AddDbContext<PostDatabaseContext>(options => options.UseSqlServer(connectionString, _ => _.CommandTimeout(600).EnableRetryOnFailure())
			.LogTo(_ => Debug.WriteLine(_), LogLevel.Warning));

		services.AddScoped<IPostDatabaseContext, PostDatabaseContext>();

		mediatRAssemblies.Add(typeof(PostsModule).Assembly);

		logger.Information("{Module} module services registered", "Posts");

		return services;
	}
}