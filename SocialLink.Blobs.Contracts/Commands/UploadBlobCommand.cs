using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Common.Application;

namespace SocialLink.Blobs.Contracts.Commands;

public sealed record UploadBlobCommand(UploadFileDto Data) : Command<UploadResult>;