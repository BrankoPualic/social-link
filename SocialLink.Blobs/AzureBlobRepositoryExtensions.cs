using SocialLink.Blobs.Contracts;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Blobs.Domain;

namespace SocialLink.Blobs;

internal static class AzureBlobRepositoryExtensions
{
	public static async Task<(Uri Uri, Cleanup Cleanup)> UploadIntoSinglesDirectoryAsync(this IAzureBlobRepository azureBlobRepository, string containerName, Guid id, FileInformationDto file)
	{
		using var stream = new MemoryStream(file.Buffer);

		return await azureBlobRepository.UploadIntoSinglesDirectoryAsync(stream, containerName, id.ToString(), file.FileName);
	}
}