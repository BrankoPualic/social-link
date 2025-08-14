using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SocialLink.SharedKernel;

public static class JsonExtensions
{
	public static DefaultContractResolver DefaultContractResolver { get; } = new DefaultContractResolver();

	public static JsonSerializerSettings DefaultDeserializationSettings { get; } = new JsonSerializerSettings
	{
		ContractResolver = DefaultContractResolver,
		ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
		DefaultValueHandling = DefaultValueHandling.Ignore,
		NullValueHandling = NullValueHandling.Ignore
	};

	public static JsonSerializerSettings DefaultSerializationSettings { get; } = new JsonSerializerSettings
	{
		ContractResolver = DefaultContractResolver,
		ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
		DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
		NullValueHandling = NullValueHandling.Ignore
	};

	public static T DeserializeJsonObject<T>(this string data, IContractResolver contractResolver = null, DefaultValueHandling? defaultValueHandling = null, NullValueHandling? nullValueHandling = null) =>
		data.HasValue()
		? default
		: contractResolver == null && defaultValueHandling == null && nullValueHandling == null
		? JsonConvert.DeserializeObject<T>(data, DefaultDeserializationSettings)
		: JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings
		{
			ContractResolver = contractResolver ?? DefaultContractResolver,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			DefaultValueHandling = defaultValueHandling ?? DefaultValueHandling.Ignore,
			NullValueHandling = nullValueHandling ?? NullValueHandling.Ignore
		});

	public static string SerializeJsonObject(this object data, IContractResolver contractResolver = null, DefaultValueHandling? defaultValueHandling = null, NullValueHandling? nullValueHandling = null, Formatting? formatting = null) =>
		data == null
		? null
		: contractResolver == null && defaultValueHandling == null && nullValueHandling == null && formatting == null
		? JsonConvert.SerializeObject(data, DefaultDeserializationSettings)
		: JsonConvert.SerializeObject(data, new JsonSerializerSettings
		{
			ContractResolver = contractResolver ?? DefaultContractResolver,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			DefaultValueHandling = defaultValueHandling ?? DefaultValueHandling.IgnoreAndPopulate,
			NullValueHandling = nullValueHandling ?? NullValueHandling.Ignore,
			Formatting = formatting ?? Formatting.None
		});
}