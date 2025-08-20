using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SocialLink.SharedKernel;
using System.Reflection;

namespace SocialLink.Web;

public class ApplicationContractResolver : DefaultContractResolver
{
	public ApplicationContractResolver()
	{
		NamingStrategy = new CamelCaseNamingStrategy
		{
			ProcessDictionaryKeys = true,
			OverrideSpecifiedNames = true,
		};
	}

	private static CustomDefaultValueConverter CustomDefaultValueConverter { get; } = new CustomDefaultValueConverter();

	protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
	{
		var property = base.CreateProperty(member, memberSerialization);

		if (property.PropertyType == typeof(DateTime)
		|| property.PropertyType == typeof(DateTime?)
		|| property.PropertyType == typeof(bool)
		|| property.PropertyType == typeof(int)
		|| property.PropertyType == typeof(long)
		|| property.PropertyType == typeof(decimal)
		|| property.PropertyType.IsEnum
		)
		{
			property.Converter ??= CustomDefaultValueConverter;
		}

		var includeDefault = property.AttributeProvider.GetAttributes(typeof(IncludeDefaultAttribute), true).Any();
		if (includeDefault
			|| property.PropertyType == typeof(DateTime)
			|| property.PropertyType == typeof(int)
			|| property.PropertyType == typeof(long)
			|| property.PropertyType == typeof(decimal)
			|| property.PropertyType.IsEnum
		)
		{
			property.DefaultValueHandling = DefaultValueHandling.Include;
		}

		return property;
	}
}