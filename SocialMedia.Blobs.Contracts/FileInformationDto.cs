namespace SocialMedia.Blobs.Contracts;

public record FileInformationDto(
	string FileName,
	string Type,
	byte[] Buffer,
	long? Size
);