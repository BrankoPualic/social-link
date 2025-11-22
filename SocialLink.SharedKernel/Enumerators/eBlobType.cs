using System.ComponentModel;

namespace SocialLink.SharedKernel.Enumerators;

public enum eBlobType
{
	[Description("Unknown")]
	Unknown = 0,

	[Description("Profile Image")]
	ProfileImage = 100,

	[Description("Post Image")]
	PostImage = 200,

	[Description("Chat Group Avatar")]
	ChatGroupAvatar = 300,

	[Description("Audio Message")]
	AudioMessage = 400
}