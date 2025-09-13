using System.ComponentModel;

namespace SocialLink.Users;

internal enum eGender
{
	Male = 1,
	Female = 2,
	Other = 3
}

internal enum eUserMedia
{
	[Description("Unknown")]
	Unknown = 0,

	[Description("Profile Image")]
	ProfileImage = 100,
}