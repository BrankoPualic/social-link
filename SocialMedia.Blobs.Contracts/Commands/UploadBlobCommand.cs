using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.SharedKernel;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Blobs.Contracts.Commands;

public sealed record UploadBlobCommand(FileInformationDto File, eBlobType BlobType) : Command<Guid>;