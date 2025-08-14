﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SocialLink.Users.Application.Interfaces;
using SocialLink.Users.Application.Services;
using SocialLink.Users.Data;
using SocialLink.Users.Domain;
using System.Diagnostics;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace SocialLink.Users;

public static class UsersModule
{
	public static IServiceCollection AddUsersModuleServices(this IServiceCollection services, IConfiguration config, ILogger logger, List<Assembly> mediatRAssemblies)
	{
		string connectionString = config.GetConnectionString("Database");
		services.AddDbContext<UserDatabaseContext>(options => options.UseSqlServer(connectionString, _ => _.CommandTimeout(600).EnableRetryOnFailure())
			.LogTo(_ => Debug.WriteLine(_), LogLevel.Warning));

		services.AddScoped<IUserDatabaseContext, UserDatabaseContext>();

		services.AddScoped<IAuthManager, AuthManager>();
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<INotificationService, NotificationService>();

		mediatRAssemblies.Add(typeof(UsersModule).Assembly);

		logger.Information("{Module} module services registered", "Users");

		return services;
	}
}