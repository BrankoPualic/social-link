using System.ComponentModel;

namespace SocialLink.SharedKernel.Enumerators;
public enum eNotificationActionMethodType
{
	[Description("")]
	NotSet = 0,

	[Description("GET")]
	Get = 100,

	[Description("POST")]
	Post = 200
}
