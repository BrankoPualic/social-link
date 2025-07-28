namespace SocialMedia.Blobs.Application.Interfaces;

internal interface IAzureBlobRepository
{
	Task<bool> DeleteAsync(string containerName, string path);

	Task<byte[]> DownloadContentAsync(string containerName, string path);

	Task<string> GetBlobSasUri(string containerName, Guid id, string blobName);

	Task<string> GetBlobSasUri(string containerName, string fileName);

	Task<(Uri Uri, Func<Task> Cleanup)> UploadIntoSinglesDirectoryAsync(Stream fileStream, string containerName, string directory, string fileName);
}