using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialLink.Blobs.Contracts.Queries;
using SocialLink.Common.Application;
using SocialLink.Messaging.Application.Dtos;
using SocialLink.Messaging.Domain.Relational;
using SocialLink.SharedKernel;
using SocialLink.Users.Contracts;

namespace SocialLink.Messaging.Application.UseCases.Queries;

internal sealed record GetConversationQuery(Guid GroupChatId) : Query<ConversationDto>;

internal class GetConversationQueryHandler(IEFMessagingDatabaseContext db, IMediator mediator) : EFQueryHandler<GetConversationQuery, ConversationDto>(db)
{
	public override async Task<ResponseWrapper<ConversationDto>> Handle(GetConversationQuery req, CancellationToken ct)
	{
		var model = await db.ChatGroupUsers
			.Where(_ => _.ChatGroupId == req.GroupChatId)
			.Where(_ => _.UserId != CurrentUser.Id)
			.Select(_ => new
			{
				_.ChatGroup,
				_.UserId,
				BlobId = _.ChatGroup.Media
									.Where(_ => _.IsActive == true)
									.Select(_ => _.BlobId)
									.FirstOrDefault()
			})
			.FirstOrDefaultAsync(ct);

		if (model is null || model.ChatGroup is null)
			return new(new Error(nameof(ChatGroup), "Conversation not found."));

		ConversationDto result = new()
		{
			Id = model.ChatGroup.Id,
			LastMessageOn = model.ChatGroup.LastMessageOn,
			LastMessagePreview = model.ChatGroup.LastMessagePreview,
		};

		if (model.ChatGroup.IsGroup != true)
		{
			var userResult = await mediator.Send(new GetUserContractQuery(model.UserId), ct);
			if (!userResult.IsSuccess)
				return new(userResult.Errors);

			result.User = userResult.Data;
		}
		else
		{
			var blobResult = await mediator.Send(new GetBlobQuery(model.BlobId), ct);
			if (!blobResult.IsSuccess)
				return new(blobResult.Errors);

			result.Name = model.ChatGroup.Name;
			result.IsGroup = true;
			result.GroupImageUrl = blobResult.Data.Url;
		}

		return new(result);
	}
}