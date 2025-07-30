using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Blobs.Contracts.Queries;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Contracts;

namespace SocialMedia.Users.Integrations;

internal class GetUsersContractQueryHandler(IUserDatabaseContext db, IMediator mediator) : EFQueryHandler<GetUsersContractQuery, List<UserContractDto>>(db)
{
	public override async Task<Result<List<UserContractDto>>> Handle(GetUsersContractQuery req, CancellationToken ct)
	{
		var userIds = req.UserIds;
		if (userIds.Count == 0)
			return Result.Invalid(new ValidationError("No user for querying."));

		var models = await db.Users
			.Where(_ => userIds.Contains(_.Id))
			.Select(_ => new UserContractDto
			{
				Id = _.Id,
				Username = _.Username,
			})
			.ToListAsync(ct);

		if (models.Count == 0)
			return Result.NotFound("Users not found.");

		var modelUserIds = models.Select(_ => _.Id).ToList();
		var blobIds = await db.Media
			.Where(_ => modelUserIds.Contains(_.UserId))
			.Where(_ => _.IsActive == true)
			.Select(_ => _.BlobId)
			.ToListAsync(ct);

		var blobsResult = await mediator.Send(new GetBlobsQuery(blobIds), ct);
		if (blobsResult.IsNotFound())
			return Result.NotFound(blobsResult.Errors.ToArray());

		foreach (var model in models)
		{
			model.ProfileImage = blobIds
				.Select(id => blobsResult.Value.FirstOrDefault(_ => _.Id == id)?.Url)
				.Where(_ => _ is not null)
				.FirstOrDefault();
		}

		return Result.Success(models);
	}
}