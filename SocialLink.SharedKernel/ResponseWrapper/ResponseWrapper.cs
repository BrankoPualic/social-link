namespace SocialLink.SharedKernel;

public class ResponseWrapper
{
	public ResponseWrapper()
	{
		IsSuccess = true;
	}

	public ResponseWrapper(Error errors)
	{
		IsSuccess = false;
		Errors = errors;
	}

	public bool IsSuccess { get; }

	public Error Errors { get; }

	public override string ToString() => !IsSuccess
		? $"ERRORS - {Errors}"
		: $"SUCCESS";
}