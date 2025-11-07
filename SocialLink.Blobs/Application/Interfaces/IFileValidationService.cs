using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel;

namespace SocialLink.Blobs.Application.Interfaces;
internal interface IFileValidationService
{
	ResponseWrapper Validate(FileInformationDto file);

	ResponseWrapper Validate(IEnumerable<FileInformationDto> files);

	ResponseWrapper Validate(string fileName, byte[] buffer);
}
