namespace SocialLink.Users.Contracts;

/// <summary>
/// This Dto should be used only externally by other modules.
/// </summary>
public class UserContractDto
{
	public Guid Id { get; set; }

	public string Username { get; set; }

	public string ProfileImage { get; set; }
}