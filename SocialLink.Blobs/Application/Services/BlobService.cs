using MongoDB.Driver;
using SocialLink.Blobs.Application.Interfaces;
using SocialLink.Blobs.Contracts;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Blobs.Domain;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Enumerators;
using SocialLink.SharedKernel.Extensions;

namespace SocialLink.Blobs.Application.Services;

internal class BlobService(IBlobDatabaseContext db, IAzureBlobRepository azureBlobRepository, IFileValidationService fileValidationService) : IBlobService
{
	private static string GetContainerName() => "blobs";

	public async Task<ResponseWrapper<string>> GetBlobSasUriAsync(Guid id, bool showAll = false)
	{
		var blob = await GetBlobAsync(id, showAll);
		if (blob is null)
			return new(new Error("File not found."));

		var container = GetContainerName();

		var result = await azureBlobRepository.GetBlobSasUri(container, id, blob.Name);

		return new(result);
	}

	public async Task<ResponseWrapper<FileInformationDto>> DownloadAsync(Guid id)
	{
		var blob = await GetBlobAsync(id, true);
		if (blob is null)
			return new(new Error("File not found."));

		var container = GetContainerName();

		var partialPath = blob.Url.GetAzureFileRelativePath(container);
		var file = await azureBlobRepository.DownloadContentAsync(container, partialPath);

		return new(new FileInformationDto(blob.Name, blob.Type, file, blob.Size));
	}

	public async Task<ResponseWrapper<(Blob blob, Cleanup Cleanup)>> UploadAsync(FileInformationDto file, Blob blob, eBlobType blobType)
	{
		var validationResult = fileValidationService.Validate(file);
		if (!validationResult.IsSuccess)
			return new(validationResult.Errors);

		var isNew = blob == null;

		blob ??= new Blob { Id = Guid.NewGuid(), IsActive = true };

		var (uri, cleanup) = await azureBlobRepository.UploadIntoSinglesDirectoryAsync(GetContainerName(), blob.Id, file);

		blob.Name = file.FileName;
		blob.TypeId = blobType;
		blob.Type = !string.IsNullOrWhiteSpace(file.Type) ? file.Type : file.FileName.ToMimeType();
		blob.Url = uri.AbsoluteUri;
		blob.Size = file.Size ?? file.Buffer.Length;

		if (isNew)
			await db.ExecuteWithAuditAsync(blob, isNew, _ => db.Blobs.InsertOneAsync(_));

		return new((blob, cleanup));
	}

	public async Task<ResponseWrapper<Guid>> DeleteAsync(Guid id)
	{
		var blob = await GetBlobAsync(id, true);
		if (blob is null)
			return new(new Error("File not found."));

		var container = GetContainerName();

		var relativePath = blob.Url.GetAzureFileRelativePath(container);

		var builder = Builders<Blob>.Filter;
		var filter = builder.Eq(_ => _.Id, id);
		await db.Blobs.DeleteOneAsync(filter);

		await azureBlobRepository.DeleteAsync(container, relativePath);

		return new(id);
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