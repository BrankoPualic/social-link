using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialLink.Blobs.Contracts.Queries;
using SocialLink.SharedKernel.UseCases;
using SocialLink.Users.Application.Dtos;

namespace SocialLink.Users.Application.UseCases.Queries;
internal sealed record GetProfileQuery(Guid UserId) : Query<UserDto>;

internal class GetProfileQueryHandler(IUserDatabaseContext db, IMediator mediator) : EFQueryHandler<GetProfileQuery, UserDto>(db)
{
	public override async Task<Result<UserDto>> Handle(GetProfileQuery req, CancellationToken ct)
	{
		var userId = req.UserId;

		var result = await db.Users
			.Where(_ => _.Id == userId)
			.Select(UserDto.Projection)
			.FirstOrDefaultAsync(ct);

		if (result is null)
			return Result.Invalid(new ValidationError("User not found"));

		var profileImageId = await db.Media
			.Where(_ => _.UserId == userId)
			.Where(_ => _.Type == eUserMedia.ProfileImage)
			.Where(_ => _.IsActive)
			.Select(_ => _.BlobId)
			.FirstOrDefaultAsync(ct);

		var blobResult = await mediator.Send(new GetBlobQuery(profileImageId), ct);
		if (blobResult.IsSuccess)
			result.ProfileImage = blobResult.Value;

		return Result.Success(result);
	}
}