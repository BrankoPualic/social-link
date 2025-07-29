using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Blobs.Contracts.Queries;
using SocialMedia.SharedKernel.UseCases;
using SocialMedia.Users.Contracts;

namespace SocialMedia.Users.Integrations;

internal class GetUserContractQueryHandler(IUserDatabaseContext db, IMediator mediator) : EFQueryHandler<GetUserContractQuery, UserContractDto>(db)
{
	public override async Task<Result<UserContractDto>> Handle(GetUserContractQuery req, CancellationToken ct)
	{
		var userId = req.UserId;

		var model = await db.Users
			.Where(_ => _.Id == userId)
			.Select(_ => new UserContractDto
			{
				Id = _.Id,
				Username = _.Username
			})
			.FirstOrDefaultAsync(ct);

		if (model is null)
			return Result.NotFound("User not found.");

		var blobId = await db.Media
			.Where(_ => _.UserId == userId)
			.Where(_ => _.IsActive == true)
			.Select(_ => _.BlobId)
			.FirstOrDefaultAsync(ct);

		var blobResult = await mediator.Send(new GetBlobQuery(blobId), ct);
		if (blobResult.IsNotFound())
			return Result.NotFound("Profile image couldn't be loaded.");

		model.ProfileImage = blobResult.Value.Url;

		return Result.Success(model);
	}
}