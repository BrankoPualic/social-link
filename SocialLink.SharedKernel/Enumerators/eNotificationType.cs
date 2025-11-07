using System.ComponentModel;

namespace SocialLink.SharedKernel.Enumerators;
public enum eNotificationType
{
	[Description("")]
	NotSet = 0,

	[Description("Follow Request")]
	FollowRequest = 10,

	[Description("Follow Accepted")]
	FollowAccepted = 20,

	[Description("Start Following")]
	StartFollowing = 30,
}