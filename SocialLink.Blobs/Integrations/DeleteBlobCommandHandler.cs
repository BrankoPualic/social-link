using MongoDB.Driver;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Domain;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.UseCases;

namespace SocialLink.Blobs.Integrations;

internal class DeleteBlobCommandHandler(IBlobDatabaseContext db) : MongoCommandHandler<DeleteBlobCommand, Guid>
{
	public override async Task<ResponseWrapper<Guid>> Handle(DeleteBlobCommand req, CancellationToken ct)
	{
		var id = req.BlobId;

		var filterBuilder = Builders<Blob>.Filter;
		var filter = filterBuilder.Eq(_ => _.Id, id);

		var updateBuilder = Builders<Blob>.Update;
		var update = updateBuilder.Set(_ => _.IsActive, false);

		update = update.WithAudit(db.CurrentUser.Id);

		var result = await db.Blobs.UpdateOneAsync(filter, update, cancellationToken: ct);

		if (result.IsAcknowledged && result.ModifiedCount > 0)
		{
			return new(id);
		}
		else
		{
			return new(new Error("File delete failed."));
		}
	}
}