using Newtonsoft.Json;

namespace SocialLink.SharedKernel;

public class CustomDefaultValueConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => serializer.Serialize(writer, value);

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		var value = serializer.Deserialize<string>(reader);

		if (string.IsNullOrEmpty(value))
			return existingValue;

		if (objectType == typeof(DateTime) || objectType == typeof(DateTime?))
		{
			if ((reader as JsonTextReader)?.ValueType != typeof(DateTime))
			{
				var valueDate = DateTime.TryParse(value, out var date) ? date : existingValue;
				return valueDate;
			}
		}

		return serializer.Deserialize(reader, objectType);
	}

	public override bool CanConvert(Type objectType) => objectType.IsValueType;
}