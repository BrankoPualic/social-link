using SocialMedia.SharedKernel;

namespace SocialMedia.Blobs.Contracts.Dtos;

public sealed record UploadFileDto(FileInformationDto File, eBlobType BlobType);