using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Serilog;
using SocialLink.Blobs;
using SocialLink.Messaging;
using SocialLink.Messaging.Hubs.Message;
using SocialLink.Messaging.Hubs.Presence;
using SocialLink.Notifications;
using SocialLink.Posts;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Domain;
using SocialLink.Users;
using SocialLink.Web;
using SocialLink.Web.Behaviors;
using SocialLink.Web.Middlewares;
using SocialLink.Web.Objects;
using System.Reflection;
using System.Text;

var logger = Log.Logger = new LoggerConfiguration()
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Configuration.AddAzureKeyVault(
	new Uri($"https://socialmedia-secrets.vault.azure.net/"),
	Settings.TokenCredential
);

builder.Services.AddCors();

builder.Services.AddHttpContextAccessor();

var jwtSettings = new JwtSettings
{
	SigningKey = builder.Configuration.GetSection(nameof(JwtSettings.SigningKey)).Value,
	Issuer = builder.Configuration.GetSection(nameof(JwtSettings.Issuer)).Value,
	Audience = builder.Configuration.GetSection(nameof(JwtSettings.Audience)).Value
};

builder.Services.Configure<JwtSettings>(opt =>
{
	opt.SigningKey = jwtSettings.SigningKey;
	opt.Issuer = jwtSettings.Issuer;
	opt.Audience = jwtSettings.Audience;
});
builder.Services.AddControllers().ConfigureApplicationPartManager(manager =>
{
	// Clear all auto-detected controllers.
	manager.ApplicationParts.Clear();

	// Add feature provider to allow "internal" controller
	manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
});

builder.Services
	.AddAuthentication(opt =>
	{
		opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey ?? throw new InvalidOperationException())),
			ValidateIssuer = true,
			ValidIssuer = jwtSettings.Issuer,
			ValidateAudience = true,
			ValidAudience = jwtSettings.Audience,
			ValidateLifetime = true,
			ClockSkew = TimeSpan.FromSeconds(60)
		};
		options.Audience = jwtSettings.Audience;
		options.ClaimsIssuer = jwtSettings.Issuer;

		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				context.Token = context.Request.Cookies[Constants.ACCESS_TOKEN_COOKIE];
				return Task.CompletedTask;
			}
		};
	});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IIdentityUser, IdentityUser>();

builder.Services.AddMemoryCache();

// Add Module Services
List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];
builder.Services.AddUsersModuleServices(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddNotificationsModuleServices(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddPostsModuleServices(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddBlobsModuleServices(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddMessagingModuleServices(builder.Configuration, logger, mediatRAssemblies);

// Set up MediatR
builder.Services.AddMediatR(_ => _.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(builder => builder
	.WithOrigins("https://localhost:4200")
	.AllowAnyHeader()
	.AllowAnyMethod()
	.AllowCredentials());

app.UseMiddleware<ExceptionMiddleware>();

// Check third party services connection
using (var scope = app.Services.CreateScope())
{
	var config = scope.ServiceProvider.GetService<IConfiguration>();
	var mongoChecker = scope.ServiceProvider.GetRequiredService<IMongoHealthChecker>();

	_ = mongoChecker.CheckAsync(config, logger);
}

BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));

app.UseAuthentication()
	.UseAuthorization();

app.MapControllers();

app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

app.Run();

// public partial class Program { } // needed for tests