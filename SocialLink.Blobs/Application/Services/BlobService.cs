using Ardalis.Result;
using MongoDB.Driver;
using SocialLink.Blobs.Application.Interfaces;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Blobs.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Blobs.Application.Services;

internal class BlobService(IBlobDatabaseContext db, IAzureBlobRepository azureBlobRepository) : IBlobService
{
	private static string GetContainerName() => "blobs";

	public async Task<Result<string>> GetBlobSasUriAsync(Guid id, bool showAll = false)
	{
		var blob = await GetBlobAsync(id, showAll);
		if (blob is null)
			return Result.NotFound("File not found.");

		var container = GetContainerName();

		var result = await azureBlobRepository.GetBlobSasUri(container, id, blob.Name);

		return Result.Success(result);
	}

	public async Task<Result<FileInformationDto>> DownloadAsync(Guid id)
	{
		var blob = await GetBlobAsync(id, true);
		if (blob is null)
			return Result.NotFound("File not found.");

		var container = GetContainerName();

		var partialPath = blob.Url.GetAzureFileRelativePath(container);
		var file = await azureBlobRepository.DownloadContentAsync(container, partialPath);

		return Result.Success(new FileInformationDto(blob.Name, blob.Type, file, blob.Size));
	}

	public async Task<(Blob blob, Func<Task> Cleanup)> UploadAsync(FileInformationDto file, Blob blob, eBlobType blobType)
	{
		var isNew = blob == null;

		blob ??= new Blob { Id = Guid.NewGuid() };

		var (uri, cleanup) = await azureBlobRepository.UploadIntoSinglesDirectoryAsync(GetContainerName(), blob.Id, file);

		blob.Name = file.FileName;
		blob.TypeId = blobType;
		blob.Type = !string.IsNullOrWhiteSpace(file.Type) ? file.Type : file.FileName.ToMimeType();
		blob.Url = uri.AbsoluteUri;
		blob.Size = file.Size ?? file.Buffer.Length;

		if (isNew)
			await db.Blobs.InsertOneAsync(blob);

		return (blob, cleanup);
	}

	public async Task<Result<Guid>> DeleteAsync(Guid id)
	{
		var blob = await GetBlobAsync(id, true);
		if (blob is null)
			return Result.NotFound("File not found.");

		var container = GetContainerName();

		var relativePath = blob.Url.GetAzureFileRelativePath(container);

		var builder = Builders<Blob>.Filter;
		var filter = builder.Eq(_ => _.Id, id);
		await db.Blobs.DeleteOneAsync(filter);

		await azureBlobRepository.DeleteAsync(container, relativePath);

		return Result.Success(id);
	}

	// private

	private async Task<Blob> GetBlobAsync(Guid id, bool showAll = false)
	{
		var builder = Builders<Blob>.Filter;
		var filter = builder.Eq(_ => _.Id, id);

		if (!showAll)
			filter &= builder.Eq(_ => _.IsActive, true);

		var blob = (await db.Blobs.FindAsync(filter)).FirstOrDefault();

		return blob;
	}
}