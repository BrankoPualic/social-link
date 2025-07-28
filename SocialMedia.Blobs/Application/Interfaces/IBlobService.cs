using Ardalis.Result;
using SocialMedia.Blobs.Contracts;
using SocialMedia.Blobs.Domain;
using SocialMedia.SharedKernel;

namespace SocialMedia.Blobs.Application.Interfaces;

internal interface IBlobService
{
	Task<Result> DeleteAsync(Guid id);

	Task<Result<FileInformationDto>> DownloadAsync(Guid id);

	Task<Result<string>> GetBlobSasUriAsync(Guid id);

	Task<(Blob blob, Func<Task> Cleanup)> UploadAsync(FileInformationDto file, Blob blob, eBlobType blobType);
}
