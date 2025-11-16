using System.ComponentModel;

namespace SocialLink.Messaging.Enumerators;

/// <summary>
/// Denormalized media type for this chat group media, mirroring the blob's type.
/// Used to simplify querying and filtering in modules without accessing the Blob entity directly.
/// When creating a blob, its type is provided, and this property maps to that type.
/// </summary>
internal enum eChatGroupMedia
{
	[Description("Unknown")]
	Unknown = 0,

	[Description("Chat Group Avatar")]
	ChatGroupAvatar = 300
}