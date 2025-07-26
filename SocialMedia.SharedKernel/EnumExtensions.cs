namespace SocialMedia.SharedKernel;

public static class EnumExtensions
{
	public static bool In<T>(this T value, params T[] args) where T : struct, Enum => args?.Contains(value) ?? false;
}