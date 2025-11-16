using System.ComponentModel;

namespace SocialLink.Users.Enumerators;

/// <summary>
/// Denormalized media type for this user media, mirroring the blob's type.
/// Used to simplify querying and filtering in modules without accessing the Blob entity directly.
/// When creating a blob, its type is provided, and this property maps to that type.
/// </summary>
internal enum eUserMedia
{
	[Description("Unknown")]
	Unknown = 0,

	[Description("Profile Image")]
	ProfileImage = 100,
}
