using System.ComponentModel;

namespace SocialMedia.SharedKernel;

public enum eSystemRole
{
	[Description("")]
	NotSet = 0,

	[Description("System Administrator")]
	SystemAdministrator = 1,

	[Description("Member")]
	Member = 2,
}