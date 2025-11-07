using MongoDB.Driver;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Domain;
using SocialLink.Common.Application;
using SocialLink.Common.Data;
using SocialLink.SharedKernel;

namespace SocialLink.Blobs.Integrations;

internal class RestoreBlobCommandHandler(IBlobDatabaseContext db) : MongoCommandHandler<RestoreBlobCommand, Guid>
{
	public override async Task<ResponseWrapper<Guid>> Handle(RestoreBlobCommand req, CancellationToken ct)
	{
		var id = req.BlobId;

		var filterBuilder = Builders<Blob>.Filter;
		var filter = filterBuilder.Eq(_ => _.Id, id);

		var updateBuilder = Builders<Blob>.Update;
		var update = updateBuilder.Set(_ => _.IsActive, true);

		update = update.WithAudit(db.CurrentUser.Id);

		var result = await db.Blobs.UpdateOneAsync(filter, update, cancellationToken: ct);

		if (result.IsAcknowledged && result.ModifiedCount > 0)
		{
			return new(id);
		}
		else
		{
			return new(new Error("File restore failed."));
		}
	}
}