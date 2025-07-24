namespace SocialMedia.Users;

public class JwtSetting
{
	public string SecretKey { get; init; }

	public int Duration { get; init; }
}