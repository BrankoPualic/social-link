namespace SocialLink.Blobs.Contracts.Dtos;

public record FileInformationDto(
	string FileName,
	string Type,
	byte[] Buffer,
	long? Size
);