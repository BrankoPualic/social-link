using Ardalis.Result;
using MediatR;
using SocialMedia.Blobs.Contracts.Commands;
using SocialMedia.Blobs.Contracts.Dtos;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.Posts.Domain;
using SocialMedia.SharedKernel;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Posts.Application.UseCases.Commands;
internal sealed record CreatePostCommand(PostEditDto Data, List<FileInformationDto> Files) : Command<Guid>;

internal class CreatePostCommandHandler(IPostDatabaseContext db, IMediator mediator) : EFCommandHandler<CreatePostCommand, Guid>(db)
{
	public override async Task<Result<Guid>> Handle(CreatePostCommand req, CancellationToken ct)
	{
		var data = req.Data;
		var files = req.Files;

		if (data is null || files.Count is 0 || (string.IsNullOrWhiteSpace(data?.Description) && files.Count is 0))
			return Result.Invalid(new ValidationError("Data is missing."));

		var model = new Post();
		data.ToModel(model);

		var uploadData = files.Select(Map()).NotNull().ToList();
		var uploadResult = await mediator.Send(new UploadBlobsCommand(uploadData), ct);
		if (!uploadResult.IsSuccess)
			return Result.Error("Something went wrong while uploading files.");

		var media = uploadResult.Value.Select((_, index) => new PostMedia
		{
			PostId = model.Id,
			BlobId = _.BlobId,
			Order = index + 1
		}).ToList();

		db.Posts.Add(model);
		db.Media.AddRange(media);
		await db.SaveChangesAsync(true, ct);

		await Task.WhenAll(uploadResult.Value.Select(_ => _.Cleanup()));

		return Result.Created(model.Id);
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
}