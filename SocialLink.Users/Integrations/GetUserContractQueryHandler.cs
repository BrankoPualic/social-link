using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialLink.Blobs.Contracts.Queries;
using SocialLink.Common.Application;
using SocialLink.SharedKernel;
using SocialLink.Users.Contracts;

namespace SocialLink.Users.Integrations;

internal class GetUserContractQueryHandler(IUserDatabaseContext db, IMediator mediator) : EFQueryHandler<GetUserContractQuery, UserContractDto>(db)
{
	public override async Task<ResponseWrapper<UserContractDto>> Handle(GetUserContractQuery req, CancellationToken ct)
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
			return new(new Error("User not found."));

		var blobId = await db.Media
			.Where(_ => _.UserId == userId)
			.Where(_ => _.IsActive == true)
			.Select(_ => _.BlobId)
			.FirstOrDefaultAsync(ct);

		var blobResult = await mediator.Send(new GetBlobQuery(blobId), ct);
		if (!blobResult.IsSuccess)
			return new(blobResult.Errors);

		model.ProfileImage = blobResult.Data.Url;

		return new(model);
	}
}