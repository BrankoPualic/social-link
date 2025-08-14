using Ardalis.Result;
using MongoDB.Driver;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Domain;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Blobs.Integrations;

internal class RestoreBlobCommandHandler(IBlobDatabaseContext db) : MongoCommandHandler<RestoreBlobCommand, Guid>
{
	public override async Task<Result<Guid>> Handle(RestoreBlobCommand req, CancellationToken ct)
	{
		var id = req.BlobId;

		var filterBuilder = Builders<Blob>.Filter;
		var filter = filterBuilder.Eq(_ => _.Id, id);

		var updateBuilder = Builders<Blob>.Update;
		var update = updateBuilder.Set(_ => _.IsActive, true);

		var result = await db.Blobs.UpdateOneAsync(filter, update, cancellationToken: ct);

		if (result.IsAcknowledged && result.ModifiedCount > 0)
		{
			return Result.Success(id);
		}
		else
		{
			return Result.Invalid(new ValidationError { ErrorMessage = "File restore failed." });
		}
	}
}