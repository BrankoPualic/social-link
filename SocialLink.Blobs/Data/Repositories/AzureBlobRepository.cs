using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using SocialLink.Blobs.Domain;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Extensions;

namespace SocialLink.Blobs.Data.Repositories;

internal class AzureBlobRepository : IAzureBlobRepository
{
	public static BlobContainerClient GetClient(string containerName, string storageName) => new(new Uri(Settings.GetBlobStorageUrl(storageName, containerName)), Settings.TokenCredential);

	private BlobContainerClient GetClient(string containerName) => GetClient(containerName, StorageName);

	protected virtual string StorageName => Settings.AzureStorageName;

	public async Task<string> GetBlobSasUri(string containerName, Guid id, string blobName) => await GetBlobSasUri(containerName, $"{id}/{blobName}");

	public async Task<string> GetBlobSasUri(string containerName, string fileName)
	{
		var blobClient = new BlobServiceClient(new Uri(Settings.GetBlobStorageUrl(StorageName)), Settings.TokenCredential);
		var key = await blobClient.GetUserDelegationKeyAsync(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(1));

		var sasBuilder = new BlobSasBuilder
		{
			BlobContainerName = containerName,
			BlobName = Uri.UnescapeDataString(fileName),
			Resource = "b",
			StartsOn = DateTimeOffset.UtcNow,
			ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
		};

		sasBuilder.SetPermissions(BlobSasPermissions.Read);
		var sasToken = sasBuilder.ToSasQueryParameters(key, StorageName).ToString();

		return Settings.GetBlobStorageUrl(StorageName, $"{containerName}/{fileName}?{sasToken}");
	}

	public async Task<(Uri Uri, Func<Task> Cleanup)> UploadIntoSinglesDirectoryAsync(Stream fileStream, string containerName, string directory, string fileName)
	{
		var client = GetClient(containerName);
		if (!await client.ExistsAsync())
			return (null, () => Task.CompletedTask);

		var blobHttpHeader = new BlobHttpHeaders { ContentType = Path.GetExtension(fileName).ToMimeType() };
		var fullPath = string.Concat(directory.TrimEnd('/'), "/", fileName.TrimStart('/'));
		var blob = client.GetBlobClient(fullPath);
		await blob.UploadAsync(fileStream, blobHttpHeader);

		return (blob.Uri, () => CleanupSinglesDirectoryAsync(containerName, directory, fileName));
	}

	public async Task<byte[]> DownloadContentAsync(string containerName, string path)
	{
		var client = GetClient(containerName);
		if (!await client.ExistsAsync())
			return default;

		var blob = client.GetBlobClient(Uri.UnescapeDataString(path));
		if (!await blob.ExistsAsync())
			return default;

		using var stream = new MemoryStream();
		await blob.DownloadToAsync(stream);

		return stream.ToArray();
	}

	public async Task<bool> DeleteAsync(string containerName, string path)
	{
		var client = GetClient(containerName);
		if (!await client.ExistsAsync())
			return false;

		var blob = client.GetBlobClient(Uri.UnescapeDataString(path));

		return await blob.DeleteIfExistsAsync();
	}

	// private

	private async Task CleanupSinglesDirectoryAsync(string containerName, string path, string fileName)
	{
		var client = GetClient(containerName);
		if (!await client.ExistsAsync())
			return;

		await foreach (var page in client.GetBlobsByHierarchyAsync(prefix: path).AsPages())
		{
			foreach (var item in page.Values)
			{
				if (string.IsNullOrWhiteSpace(fileName) || !item.Blob.Name.EndsWith(fileName))
					await client.DeleteBlobAsync(item.Blob.Name);
			}
		}
	}
}