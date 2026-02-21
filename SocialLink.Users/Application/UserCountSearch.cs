using SocialLink.Users.Enumerators;

namespace SocialLink.Users.Application;

internal sealed record UserCountSearch
{
	public bool IsLocked { get; set; }

	public bool IsActive { get; set; }

	public eGender? GenderId { get; set; }
}