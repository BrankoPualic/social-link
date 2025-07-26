using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Serilog;
using SocialMedia.Notifications;
using SocialMedia.SharedKernel;
using SocialMedia.Users;
using SocialMedia.Web.Objects;
using System.Reflection;

var logger = Log.Logger = new LoggerConfiguration()
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));

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

// Set up MediatR
builder.Services.AddMediatR(_ => _.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()));

var app = builder.Build();

app.UseAuthentication()
	.UseAuthorization()
	.UseFastEndpoints()
	.UseSwaggerGen(); // once application runs goto /swagger

app.Run();

// public partial class Program { } // needed for tests