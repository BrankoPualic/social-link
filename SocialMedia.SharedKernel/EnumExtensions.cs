using System.ComponentModel;
using System.Reflection;

namespace SocialMedia.SharedKernel;

public static class EnumExtensions
{
	public static bool In<T>(this T value, params T[] args) where T : struct, Enum => args?.Contains(value) ?? false;

	public static string GetDescription(this Enum value)
	{
		FieldInfo field = value.GetType().GetField(value.ToString());

		return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute attribute
			? value.ToString()
			: attribute.Description;
	}
}