using FluentValidation;
using MediatR;
using SocialLink.Web.Exceptions;

namespace SocialLink.Web.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var context = new ValidationContext<TRequest>(request);

		var failures = validators
			.Select(_ => _.Validate(context))
			.SelectMany(_ => _.Errors)
			.Where(_ => _ is not null)
			.ToList();

		if (failures.Count is not 0)
			throw new FluentValidationException(failures);

		return await next(cancellationToken);
	}
}