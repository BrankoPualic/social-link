using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Blobs.Contracts.Commands;

public sealed record UploadBlobsCommand(List<UploadFileDto> Data) : Command<List<UploadResult>>;