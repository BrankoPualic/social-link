using System.ComponentModel;

namespace SocialLink.SharedKernel.Enumerators;
public enum eSystemRole
{
	[Description("")]
	NotSet = 0,

	[Description("System Administrator")]
	SystemAdministrator = 1,

	[Description("Member")]
	Member = 2,
}

