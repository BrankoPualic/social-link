using Ardalis.Result;
using MongoDB.Driver;
using SocialMedia.Blobs.Contracts.Commands;
using SocialMedia.Blobs.Domain;
using SocialMedia.SharedKernel.UseCases;

namespace SocialMedia.Blobs.Integrations;

internal class DeleteBlobCommandHandler(IBlobDatabaseContext db) : MongoCommandHandler<DeleteBlobCommand, Guid>
{
	public override async Task<Result<Guid>> Handle(DeleteBlobCommand req, CancellationToken ct)
	{
		var id = req.BlobId;

		var filterBuilder = Builders<Blob>.Filter;
		var filter = filterBuilder.Eq(_ => _.Id, id);

		var updateBuilder = Builders<Blob>.Update;
		var update = updateBuilder.Set(_ => _.IsActive, false);

		var result = await db.Blobs.UpdateOneAsync(filter, update, cancellationToken: ct);

		if (result.IsAcknowledged && result.ModifiedCount > 0)
		{
			return Result.Success(id);
		}
		else
		{
			return Result.Invalid(new ValidationError { ErrorMessage = "File delete failed." });
		}
	}
}