using SocialMedia.Blobs.Application.Interfaces;
using SocialMedia.Blobs.Contracts;

namespace SocialMedia.Blobs;

internal static class AzureBlobRepositoryExtensions
{
	public static async Task<(Uri Uri, Func<Task> Cleanup)> UploadIntoSinglesDirectoryAsync(this IAzureBlobRepository azureBlobRepository, string containerName, Guid id, FileInformationDto file)
	{
		using var stream = new MemoryStream(file.Buffer);

		return await azureBlobRepository.UploadIntoSinglesDirectoryAsync(stream, containerName, id.ToString(), file.FileName);
	}
}