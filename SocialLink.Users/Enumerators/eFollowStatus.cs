using System.ComponentModel;

namespace SocialLink.Users.Enumerators;
internal enum eFollowStatus
{
	[Description("Unknown")]
	Unknown = 0,

	[Description("Pending")]
	Pending = 1,

	[Description("Active")]
	Active = 2
}