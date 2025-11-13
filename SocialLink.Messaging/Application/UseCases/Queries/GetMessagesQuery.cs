using MediatR;
using MongoDB.Driver;
using SocialLink.Common.Application;
using SocialLink.Common.Data;
using SocialLink.Messaging.Application.Dtos;
using SocialLink.Messaging.Application.UseCases.Commands;
using SocialLink.Messaging.Domain.Document;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Extensions;
using SocialLink.Users.Contracts;

namespace SocialLink.Messaging.Application.UseCases.Queries;

internal sealed record GetMessagesQuery(MessageSearch Search) : Query<PagedResponse<MessageDto>>;

internal class GetMessagesQueryHandler(IMongoMessagingDatabaseContext db, IMediator mediator) : MongoQueryHandler<GetMessagesQuery, PagedResponse<MessageDto>>
{
	public override async Task<ResponseWrapper<PagedResponse<MessageDto>>> Handle(GetMessagesQuery req, CancellationToken ct)
	{
		var search = req.Search;

		var builder = Builders<Message>.Filter;
		var filter = builder.Eq(_ => _.ChatGroupId, search.ChatGroupId);

		var result = await db.Messages.MongoSearchAsync(
			search,
			_ => _.CreatedOn,
			true,
			MessageDto.Projection,
			filter,
			ct
		);

		if (result.TotalCount is 0)
			return new();

		var userIds = result.Items.SelectIds(_ => _.UserId);

		var usersResult = await mediator.Send(new GetUsersContractQuery(userIds), ct);
		if (!usersResult.IsSuccess)
			return new(usersResult.Errors);

		var usersMap = usersResult.Data.ToDictionary(_ => _.Id);

		foreach (var message in result.Items)
		{
			message.User = usersMap.GetValueOrDefault(message.UserId);
		}

		await mediator.Send(new ReadMessageCommand(new ReadMessageDto
		{
			LastMessageId = result.Items.First().Id,
			ChatGroupId = search.ChatGroupId,
			UserId = db.CurrentUser.Id,
		}), ct);

		return new(result);
	}
}