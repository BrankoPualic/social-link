using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Serilog;
using SocialLink.Blobs;
using SocialLink.Notifications;
using SocialLink.Posts;
using SocialLink.SharedKernel;
using SocialLink.Users;
using SocialLink.Web.Binders;
using SocialLink.Web.Middlewares;
using SocialLink.Web.Objects;
using System.Reflection;

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

builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddSingleton(typeof(IRequestBinder<>), typeof(JsonModelBinder<>));
builder.Services
	.AddAuthenticationJwtBearer(_ => _.SigningKey = builder.Configuration["Jwt:SecretKey"])
	.AddAuthorization()
	.AddFastEndpoints()
	.SwaggerDocument();

builder.Services.AddScoped<IIdentityUser, IdentityUser>();

// Add Module Services
List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];
builder.Services.AddUsersModuleServices(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddNotificationsModuleServices(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddPostsModuleServices(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddBlobsModuleServices(builder.Configuration, logger, mediatRAssemblies);

// Set up MediatR
builder.Services.AddMediatR(_ => _.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Check third party services connection
using (var scope = app.Services.CreateScope())
{
	var config = scope.ServiceProvider.GetService<IConfiguration>();
	var mongoChecker = scope.ServiceProvider.GetRequiredService<IMongoHealthChecker>();

	_ = mongoChecker.CheckAsync(config, logger);
}

BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));

app.UseCors(builder => builder
	.AllowAnyHeader()
	.AllowAnyMethod()
	.AllowAnyOrigin());

app.UseAuthentication()
	.UseAuthorization()
	.UseFastEndpoints()
	.UseSwaggerGen(); // once application runs goto /swagger

app.Run();

// public partial class Program { } // needed for tests