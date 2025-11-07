using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Blobs.Domain;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Enumerators;

namespace SocialLink.Blobs.Application.Interfaces;

internal interface IBlobService
{
	Task<ResponseWrapper<string>> GetBlobSasUriAsync(Guid id, bool showAll = false);

	Task<(Blob blob, Func<Task> Cleanup)> UploadAsync(FileInformationDto file, Blob blob, eBlobType blobType);

	Task<ResponseWrapper<FileInformationDto>> DownloadAsync(Guid id);

	Task<ResponseWrapper<Guid>> DeleteAsync(Guid id);
}