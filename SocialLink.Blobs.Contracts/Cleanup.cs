namespace SocialLink.Blobs.Contracts;
public class Cleanup
{
	public Cleanup() => Action = () => Task.CompletedTask;

	public Cleanup(Func<Task> action) => Action = action;

	public Func<Task> Action { get; private set; }

	public Task ExecuteAsync() => Action();
}