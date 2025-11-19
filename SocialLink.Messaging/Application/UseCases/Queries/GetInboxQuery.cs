using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialLink.Blobs.Contracts.Queries;
using SocialLink.Common.Application;
using SocialLink.Common.Data;
using SocialLink.Messaging.Application.Dtos;
using SocialLink.Messaging.Domain.Relational;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Extensions;
using SocialLink.Users.Contracts;
using System.Linq.Expressions;

namespace SocialLink.Messaging.Application.UseCases.Queries;

internal sealed record GetInboxQuery(InboxSearch Search) : Query<PagedResponse<ConversationDto>>;

internal class GetInboxQueryHandler(IEFMessagingDatabaseContext db, IMediator mediator) : EFQueryHandler<GetInboxQuery, PagedResponse<ConversationDto>>(db)
{
	public override async Task<ResponseWrapper<PagedResponse<ConversationDto>>> Handle(GetInboxQuery req, CancellationToken ct)
	{
		var search = req.Search;

		var filters = new List<Expression<Func<ChatGroup, bool>>>
		{
			_ => _.Users.Any(_ => _.UserId == search.UserId)
		}; ;

		var result = await db.ChatGroups.EFSearchAsync(
			search,
			_ => _.LastMessageOn,
			true,
			ConversationDto.Projection,
			filters,
			ct
		);

		if (result.TotalCount is 0)
			return new();

		var chatGroupIds = result.Items.Where(_ => _.IsGroup != true).SelectIds(_ => _.Id);

		var chatGroupUserIdsMap = await db.ChatGroupUsers
			.Where(_ => chatGroupIds.Contains(_.ChatGroupId))
			.Where(_ => _.UserId != search.UserId)
			.ToDictionaryAsync(_ => _.ChatGroupId, _ => _.UserId, ct);

		var userIds = chatGroupUserIdsMap.Values.ToList();

		var usersResult = await mediator.Send(new GetUsersContractQuery(userIds), ct);
		if (!usersResult.IsSuccess)
			return new(usersResult.Errors);

		var usersMap = usersResult.Data.ToDictionary(_ => _.Id);

		var groupChatIds = result.Items.Where(_ => _.IsGroup == true).SelectIds(_ => _.Id);
		var groupMediaMap = await db.Media
			.Where(_ => groupChatIds.Contains(_.ChatGroupId))
			.Where(_ => _.IsActive == true)
			.ToDictionaryAsync(_ => _.ChatGroupId, _ => _.BlobId, ct);

		var blobsResult = await mediator.Send(new GetBlobsQuery(groupMediaMap.Values.ToList()), ct);
		if (!blobsResult.IsSuccess)
			return new(blobsResult.Errors);

		foreach (var chatGroup in result.Items)
		{
			if (chatGroupUserIdsMap.TryGetValue(chatGroup.Id, out var userId))
			{
				chatGroup.User = usersMap.GetValueOrDefault(userId);
			}

			if (groupMediaMap.TryGetValue(chatGroup.Id, out var blobId))
			{
				chatGroup.GroupImageUrl = blobsResult.Data.FirstOrDefault(_ => _.Id == blobId).Url;
			}
		}

		return new(result);
	}
}