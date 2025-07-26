﻿namespace SocialMedia.SharedKernel;

public static class StringExtensions
{
	public static bool HasValue(this string value) => !string.IsNullOrWhiteSpace(value);
}