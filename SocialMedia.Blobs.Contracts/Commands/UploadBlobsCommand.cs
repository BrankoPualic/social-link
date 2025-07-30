using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Blobs.Contracts.Commands;

public sealed record UploadBlobsCommand(List<UploadFileDto> Data) : Command<List<UploadResult>>;