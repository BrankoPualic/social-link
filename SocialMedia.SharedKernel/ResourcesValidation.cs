using Humanizer;

namespace SocialMedia.SharedKernel;

public static class ResourcesValidation
{
	public static string Required(string key) => "{0} is required.".FormatWith(key).Humanize();

	public static string MaximumLength(string key, int length) => "{0} cannot be more than {1} characters.".FormatWith(key, length).Humanize();

	public static string MinimumLength(string key, int length) => "{0} cannot be less than {1} characters.".FormatWith(key, length).Humanize();

	public static string WrongFormat(string key) => "{0} is in the wrong format.".FormatWith(key).Humanize();

	public static string InvalidValue(string key) => "{0} value must be valid.".FormatWith(key).Humanize();
}