using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace SocialMedia.Users;

public static class UsersModule
{
	public static IServiceCollection AddUsersModuleServices(this IServiceCollection services, ILogger logger, List<Assembly> mediatRAssemblies)
	{
		mediatRAssemblies.Add(typeof(UsersModule).Assembly);

		logger.Information("{Module} module services registered", "Users");

		return services;
	}
}