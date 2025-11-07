using HeyRed.Mime;
using SocialLink.SharedKernel.Domain;

namespace SocialLink.SharedKernel.Extensions;

public static class DataExtensions
{
	public static void GenerateIdIfNew(this IDomainModel<Guid> model)
	{
		if (model.IsNew)
			model.Id = Guid.NewGuid();
	}

	public static string ToMimeType(this string value) => MimeTypesMap.GetMimeType(value);

	public static string GetAzureFileRelativePath(this string file, string containerName) => file[(file.IndexOf(containerName) + containerName.Length + 1)..];
}