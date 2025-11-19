using SocialLink.Blobs.Application.Interfaces;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.SharedKernel;

namespace SocialLink.Blobs.Application.Services;

internal class FileValidationService : IFileValidationService
{
	//
	// more at: https://en.wikipedia.org/wiki/List_of_file_signatures
	//
	private static readonly List<(List<string> Ext, List<(int Skip, byte[] Bytes)> Signatures)> AllowedFileSignatures = [
		//
		// Images
		//

		( [".png", ".jpeg", ".jpg", ".jfif"], [
			(0, "RIFF"u8.ToArray()),
			(6, "JFIF"u8.ToArray()),

			(0, [ 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A ]),

			(0, [ 0xFF, 0xD8, 0xFF, 0xDB ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xE0 ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xE1 ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xE2 ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xE3 ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xE4 ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xE5 ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xE6 ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xE7 ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xE8 ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xEA ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xEB ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xEC ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xED ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xEE ]),
			(0, [ 0xFF, 0xD8, 0xFF, 0xEF ])
		]),

		( [".tif", ".tiff", ".dng"], [
			(0, [ 0x49, 0x49, 0x2A, 0x00 ]),
			(0, [ 0x49, 0x49, 0x2B, 0x00 ]),
			(0, [ 0x4D, 0x4D, 0x00, 0x2A ]),
			(0, [ 0x4D, 0x4D, 0x00, 0x2B ]),
		]),

		( [".bmp"], [
			(0, "BM"u8.ToArray())
		]),

		( [".heic"], [
			(4, "ftypheic"u8.ToArray()),
			(4, "ftypmif1"u8.ToArray()),
			(4, "ftypheix"u8.ToArray()),
			(4, "ftyphevc"u8.ToArray()),
			(4, "ftyphev1"u8.ToArray()),
		]),

		( [".webp"], [
			(0, "RIFF"u8.ToArray())
		]),

		// Video

		( [".mp4", ".avif"], [
			(4, "ftyp"u8.ToArray())
		]),
	];

	private readonly string ErrorPrefix = "File";

	public ResponseWrapper Validate(FileInformationDto file)
	{
		return file is null
			? new()
			: Validate(file.FileName, file.Buffer);
	}

	public ResponseWrapper Validate(IEnumerable<FileInformationDto> files)
	{
		foreach (var file in files)
		{
			var result = Validate(file);
			if (!result.IsSuccess)
				return new(result.Errors);
		}

		return new();
	}

	public ResponseWrapper Validate(string fileName, byte[] buffer)
	{
		var ext = Path.GetExtension(fileName)?.ToLower();

		var signatures = AllowedFileSignatures
			.Where(_ => _.Ext.Contains(ext))
			.SelectMany(_ => _.Signatures)
			.ToList();

		if (signatures.Count == 0)
			return new(new Error(ErrorPrefix, $"{fileName} | File's extension '{ext}' is not permitted"));

		if (buffer.Length == 0)
			return new();

		var headerBytes = buffer.Take(signatures.Max(_ => _.Skip + _.Bytes.Length)).ToArray();

		var isValidSignature = signatures.Any(_ =>
			headerBytes.Length >= _.Skip + _.Bytes.Length &&
			headerBytes.Skip(_.Skip).Take(_.Bytes.Length).SequenceEqual(_.Bytes));

		if (!isValidSignature)
			return new(new Error(ErrorPrefix, $"{fileName} | File's signature does not match the expected signature for '{ext}'"));

		var maxSize = Math.Round(Settings.MaxFileSizeInMB * 1024.0d * 1024.0d, 1);

		if (buffer.Length > maxSize)
			return new(new Error(ErrorPrefix, $"{fileName} | File exceeds the allowed size limit of {maxSize} MB"));

		return new();
	}
}