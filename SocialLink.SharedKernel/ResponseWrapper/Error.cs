namespace SocialLink.SharedKernel;

public class Error
{
	public List<KeyValuePair<string, string>> Errors { get; }

	public bool HasErrors => Errors.Any();

	public Error() => Errors = [];

	public Error(string message) : this() => Errors.Add(new("Error", message));

	public Error(string key, string message) : this() => Errors.Add(new(key, message));

	public Error(string key, List<string> messages) : this() => messages.ForEach(_ => Errors.Add(new(key, _)));
}