using FastEndpoints;
using Newtonsoft.Json;

namespace SocialLink.Web.Binders;

public class JsonModelBinder<TRequest> : IRequestBinder<TRequest> where TRequest : notnull, new()
{
	private static JsonSerializerSettings DeserializationSettings { get; } = new()
	{
		ContractResolver = new ApplicationContractResolver(),
		ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
		DefaultValueHandling = DefaultValueHandling.Ignore,
		NullValueHandling = NullValueHandling.Ignore,
	};

	public async ValueTask<TRequest> BindAsync(BinderContext ctx, CancellationToken ct)
	{
		var req = ctx.HttpContext.Request;

		if (req.HasJsonContentType())
		{
			using var reader = new StreamReader(ctx.HttpContext.Request.Body);
			var body = await reader.ReadToEndAsync(ct);

			if (!string.IsNullOrWhiteSpace(body))
			{
				var result = JsonConvert.DeserializeObject<TRequest>(body, DeserializationSettings);
				if (result is not null)
					return result;
			}
		}

		if (req.HasFormContentType)
		{
			var form = await req.ReadFormAsync(ct);
			if (form.TryGetValue("model", out var json))
			{
				var result = JsonConvert.DeserializeObject<TRequest>(json!, DeserializationSettings);
				if (result is not null)
					return result;
			}
		}

		return new();
	}
}