using SocialLink.SharedKernel.Enumerators;

namespace SocialLink.Blobs.Contracts.Dtos;

public sealed record UploadFileDto(FileInformationDto File, eBlobType BlobType);