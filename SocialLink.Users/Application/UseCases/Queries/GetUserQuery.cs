using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialLink.Blobs.Contracts.Queries;
using SocialLink.Common.Application;
using SocialLink.Posts.Contracts;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Enumerators;

namespace SocialLink.Users.Application.UseCases.Queries;

internal sealed record GetUserQuery(Guid UserId) : Query<UserDto>;

internal class GetUserQueryHandler(IUserDatabaseContext db, IMediator mediator) : EFQueryHandler<GetUserQuery, UserDto>(db)
{
	public override async Task<ResponseWrapper<UserDto>> Handle(GetUserQuery req, CancellationToken ct)
	{
		var userId = req.UserId;

		var result = await db.Users
			.Where(_ => _.Id == userId)
			.Select(UserDto.Projection)
			.FirstOrDefaultAsync(ct);

		if (result is null)
			return new(new Error("User not found."));

		var profileImageId = await db.Media
			.Where(_ => _.UserId == userId)
			.Where(_ => _.Type == eUserMedia.ProfileImage)
			.Where(_ => _.IsActive)
			.Select(_ => _.BlobId)
			.FirstOrDefaultAsync(ct);

		var blobResult = await mediator.Send(new GetBlobQuery(profileImageId), ct);
		if (blobResult.IsSuccess)
			result.ProfileImage = blobResult.Data;

		var follows = await db.Follows
			.Where(_ => _.FollowerId == userId || _.FollowingId == userId)
			.Where(_ => !_.IsPending)
			.GroupBy(_ => 1)
			.Select(g => new
			{
				Followers = g.Count(_ => _.FollowingId == userId),
				Following = g.Count(_ => _.FollowerId == userId)
			})
			.FirstOrDefaultAsync(ct);

		result.Followers = follows?.Followers ?? 0;
		result.Following = follows?.Following ?? 0;

		result.Posts = (await mediator.Send(new GetUserPostCountQuery(userId), ct))?.Data;

		return new(result);
	}
}