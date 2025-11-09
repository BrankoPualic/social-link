using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SocialLink.Posts.Application.UseCases.Commands;
using SocialLink.Posts.Application.UseCases.Validators;
using SocialLink.Posts.Data;
using SocialLink.Posts.Domain;
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

		services.AddControllers()
			.PartManager.ApplicationParts.Add(new AssemblyPart(typeof(PostsModule).Assembly));

		services.AddScoped<IPostDatabaseContext, PostDatabaseContext>();
		services.AddScoped<IPostRepository, PostRepository>();

		// Validators
		services.AddTransient<IValidator<CreatePostCommand>, CreatePostCommandValidator>();
		services.AddTransient<IValidator<UpdatePostCommand>, UpdatePostCommandValidator>();
		services.AddTransient<IValidator<UpdatePostLikeStatusCommand>, UpdatePostLikeStatusCommandValidator>();
		services.AddTransient<IValidator<CreateCommentCommand>, CreateCommentCommandValidator>();
		services.AddTransient<IValidator<UpdateCommentCommand>, UpdateCommentCommandValidator>();
		services.AddTransient<IValidator<UpdateCommentLikeStatusCommand>, UpdateCommentLikeStatusCommandValidator>();


		mediatRAssemblies.Add(typeof(PostsModule).Assembly);

		logger.Information("{Module} module services registered", "Posts");

		return services;
	}
}