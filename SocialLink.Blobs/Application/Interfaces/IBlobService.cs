using Ardalis.Result;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Blobs.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Blobs.Application.Interfaces;

internal interface IBlobService
{
	Task<Result<string>> GetBlobSasUriAsync(Guid id, bool showAll = false);

	Task<(Blob blob, Func<Task> Cleanup)> UploadAsync(FileInformationDto file, Blob blob, eBlobType blobType);

	Task<Result<FileInformationDto>> DownloadAsync(Guid id);

	Task<Result<Guid>> DeleteAsync(Guid id);
}