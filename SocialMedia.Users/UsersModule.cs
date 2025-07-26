using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SocialMedia.Users.Application.Interfaces;
using SocialMedia.Users.Application.Services;
using SocialMedia.Users.Data;
using SocialMedia.Users.Domain;
using System.Reflection;

namespace SocialMedia.Users;

public static class UsersModule
{
	public static IServiceCollection AddUsersModuleServices(this IServiceCollection services, IConfiguration config, ILogger logger, List<Assembly> mediatRAssemblies)
	{
		string? connectionString = config.GetConnectionString("Database");
		services.AddDbContext<UserDatabaseContext>(options => options.UseSqlServer(connectionString));

		services.AddScoped<IDatabaseContext, UserDatabaseContext>();

		services.AddScoped<IAuthManager, AuthManager>();
		services.AddScoped<IUserRepository, UserRepository>();

		mediatRAssemblies.Add(typeof(UsersModule).Assembly);

		logger.Information("{Module} module services registered", "Users");

		return services;
	}
}