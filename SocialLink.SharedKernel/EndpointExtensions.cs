using Ardalis.Result;
using FastEndpoints;
using FluentValidation.Results;

using Microsoft.AspNetCore.Http;

namespace SocialLink.SharedKernel;

public static class EndpointExtensions
{
	public static async Task SendResponseAsync<T>(this Result<T> result, HttpContext httpContext, CancellationToken ct = default)
	{
		var task = result.Status switch
		{
			ResultStatus.Ok => httpContext.Response.SendOkAsync(result.Value, cancellation: ct),
			ResultStatus.Created => httpContext.Response.SendCreatedAtAsync("", responseBody: result.Value, cancellation: ct),
			ResultStatus.NoContent => httpContext.Response.SendNoContentAsync(ct),
			ResultStatus.NotFound => httpContext.Response.SendNotFoundAsync(ct),
			ResultStatus.Invalid => result.SendValidationErrorsAsync(httpContext, ct: ct),
			ResultStatus.Unauthorized => httpContext.Response.SendUnauthorizedAsync(ct),
			ResultStatus.Forbidden => httpContext.Response.SendForbiddenAsync(ct),
			_ => throw new ArgumentException($"Unsupported ResultStatus: {result.Status}")
		};
		await task;
	}

	private static Task SendValidationErrorsAsync<T>(this Result<T> result, HttpContext httpContext, int statusCode = 400, CancellationToken ct = default)
	{
		if (result.ValidationErrors is null || !result.ValidationErrors.Any())
			return Task.CompletedTask;

		var failures = result.ValidationErrors
			.Select(_ => new ValidationFailure(_.Identifier, _.ErrorMessage))
			.ToList();

		// TODO: If validation error do not provide key, this line throws exception.
		// TODO: Find and fix all validation errors without keys
		return httpContext.Response.SendErrorsAsync(failures, statusCode, null, ct);
	}
}