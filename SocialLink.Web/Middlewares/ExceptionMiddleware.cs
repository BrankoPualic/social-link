using Ardalis.Result;
using Newtonsoft.Json;
using SocialLink.SharedKernel;
using System.Net;

namespace SocialLink.Web.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
	public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex, logger);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ExceptionMiddleware> logger)
	{
		logger.LogError(ex, "{message}", ex.Message);
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = ex switch
		{
			_ => (int)HttpStatusCode.InternalServerError
		};

		var response = Result.CriticalError("Something went wrong. Please contact your system administrator.");

		var json = response.SerializeJsonObject(formatting: Formatting.Indented);

		return context.Response.WriteAsync(json);
	}
}