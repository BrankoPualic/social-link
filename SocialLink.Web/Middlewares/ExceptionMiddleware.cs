using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Extensions;
using SocialLink.Users.Exceptions;
using SocialLink.Web.Exceptions;
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

		var (statusCode, errorResponse) = GetExceptionDetails(ex);

		context.Response.StatusCode = (int)statusCode;

		var json = errorResponse.SerializeJsonObject(
			contractResolver: new CamelCasePropertyNamesContractResolver(),
			formatting: Formatting.Indented
		);

		return context.Response.WriteAsync(json);
	}

	private static (HttpStatusCode statusCode, Error error) GetExceptionDetails(Exception exception)
	{
		Error HandleFluentValidationException(FluentValidationException e)
		{
			Error errorResponse = new();
			foreach (var error in e.Failures)
			{
				errorResponse.Add(error.Key, error.Value.ToList());
			}

			return errorResponse;
		}
		;

		return exception switch
		{
			RefreshTokenException => (HttpStatusCode.Forbidden, new(exception.Message)),
			FluentValidationException => (HttpStatusCode.BadRequest, HandleFluentValidationException(exception as FluentValidationException)),
			_ => (HttpStatusCode.InternalServerError, new("Something went wrong. Please contact your system administrator."))
		};
	}
}