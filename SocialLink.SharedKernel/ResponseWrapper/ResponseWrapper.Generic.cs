namespace SocialLink.SharedKernel;

public class ResponseWrapper<T> : ResponseWrapper
{
	private readonly T _data;

	public ResponseWrapper(Error errors) : base(errors)
	{
	}

	public ResponseWrapper(T data) : base() => _data = data;

	public T Data => IsSuccess ? _data : throw new InvalidOperationException("The value of a response cannot be accessed.");
}