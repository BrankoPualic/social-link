using FluentValidation.Results;

namespace SocialLink.Web.Exceptions;

public class FluentValidationException : Exception
{
	public IDictionary<string, string[]> Failures { get; }

	public string Type { get; set; } = string.Empty;

	public FluentValidationException()
		: base("One or more validation failures have occurred")
		=> Failures = new Dictionary<string, string[]>();

	public FluentValidationException(IEnumerable<ValidationFailure> failures)
		: this()
	{
		var propertyNames = failures
			.Select(_ => _.PropertyName)
			.Distinct();

		foreach (var propertyName in propertyNames)
		{
			var propertyFailures = failures
				.Where(_ => _.PropertyName.Equals(propertyName))
				.Select(_ => _.ErrorMessage)
				.ToArray();

			var removeString = ".";
			var index = propertyName.IndexOf(removeString);
			var name = (index < 0) ? propertyName : propertyName.Substring(index + 1);

			Failures.Add(name, propertyFailures);
		}
	}

	public FluentValidationException(string propertyName, string error)
		: this()
	{
		Failures.Add(propertyName, [error]);
	}
}