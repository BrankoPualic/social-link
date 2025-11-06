using MediatR;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Posts.Application.UseCases.Commands;
internal sealed record CreatePostCommand(PostCreateDto Data, List<FileInformationDto> Files) : Command<Guid>;

internal class CreatePostCommandHandler(IPostDatabaseContext db, IMediator mediator, IPostRepository postRepository) : EFCommandHandler<CreatePostCommand, Guid>(db)
{
	public override async Task<ResponseWrapper<Guid>> Handle(CreatePostCommand req, CancellationToken ct)
	{
		var data = req.Data;
		var files = req.Files;

		if (data is null || files.Count is 0 || string.IsNullOrWhiteSpace(data?.Description) && files.Count is 0)
			return new(new Error(nameof(Post), "Data is missing."));

		var model = new Post();
		data.ToModel(model);

		var uploadData = files.Select(Map()).NotNull().ToList();
		var uploadResult = await mediator.Send(new UploadBlobsCommand(uploadData), ct);
		if (!uploadResult.IsSuccess)
			return new(new Error(nameof(Post), "Something went wrong while uploading files."));


		foreach (var (uploadedMedia, index) in uploadResult.Data.WithIndex())
		{
			// TODO: How to know which type we uploaded ???
			// ANSWER: Because on upload blobs we use Task.WhenAll. We preserve order of uploaded data.
			// Which means we can find type based of index
			postRepository.CreateMedia(
				model.Id,
				uploadedMedia.BlobId,
				MapMediaType(uploadData[index].BlobType),
				index + 1
			);
		}

		db.Posts.Add(model);
		await db.SaveChangesAsync(true, ct);

		await Task.WhenAll(uploadResult.Data.Select(_ => _.Cleanup()));

		return new(model.Id);
	}

	private static Func<FileInformationDto, UploadFileDto> Map() => _ =>
	{
		var type = eBlobType.Unknown;
		if (_.Type?.StartsWith("image/") is true)
			type = eBlobType.PostImage;

		if (type is eBlobType.Unknown)
			return null;

		var model = new UploadFileDto(_, type);
		return model;
	};

	private static ePostMedia MapMediaType(eBlobType type) => type switch
	{
		eBlobType.PostImage => ePostMedia.PostImage,
		_ => ePostMedia.Unknown
	};
}