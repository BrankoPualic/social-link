using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SocialMedia.Users.Application.Interfaces;
using SocialMedia.Users.Application.Services;
using SocialMedia.Users.Data;
using SocialMedia.Users.Domain;
using System.Diagnostics;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace SocialMedia.Users;

public static class UsersModule
{
	public static IServiceCollection AddUsersModuleServices(this IServiceCollection services, IConfiguration config, ILogger logger, List<Assembly> mediatRAssemblies)
	{
		string? connectionString = config.GetConnectionString("Database");
		services.AddDbContext<UserDatabaseContext>(options => options.UseSqlServer(connectionString, _ => _.CommandTimeout(600).EnableRetryOnFailure())
			.LogTo(_ => Debug.WriteLine(_), LogLevel.Warning));

		services.AddScoped<IUserDatabaseContext, UserDatabaseContext>();

		services.AddScoped<IAuthManager, AuthManager>();
		services.AddScoped<IUserRepository, UserRepository>();

		mediatRAssemblies.Add(typeof(UsersModule).Assembly);

		logger.Information("{Module} module services registered", "Users");

		return services;
	}
}