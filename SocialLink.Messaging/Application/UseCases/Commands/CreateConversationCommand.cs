using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.Messaging.Application.Dtos;
using SocialLink.Messaging.Domain.Relational;
using SocialLink.SharedKernel;

namespace SocialLink.Messaging.Application.UseCases.Commands;

internal sealed record CreateConversationCommand(ConversationCreateDto Data) : Command<Guid>;

internal class CreateConversationCommandHandler(IEFMessagingDatabaseContext db) : EFCommandHandler<CreateConversationCommand, Guid>(db)
{
	public override async Task<ResponseWrapper<Guid>> Handle(CreateConversationCommand req, CancellationToken ct)
	{
		var data = req.Data;

		if (data is null || data.Users.Count is 0)
			return new(new Error(nameof(ChatGroup), "No user provided."));

		// TODO: When we add groups just check dto flag for IsGroup
		// 1 on 1 conversations
		var existingChatGroupId = await db.ChatGroups
			.Where(_ => _.Users.Count == 2)
			.Where(_ => _.Users.Any(_ => _.UserId == data.Users[0]))
			.Where(_ => _.Users.Any(_ => _.UserId == data.Users[1]))
			.Select(_ => _.Id)
			.FirstOrDefaultAsync(ct);

		if (existingChatGroupId != Guid.Empty)
			return new(existingChatGroupId);

		var model = new ChatGroup
		{
			Id = Guid.NewGuid(),
			Users = data.Users.Select(userId => new ChatGroupUser
			{
				UserId = userId,
				JoinedOn = DateTime.UtcNow,
				LastChangedOn = DateTime.UtcNow,
			})
			.ToList()
		};

		db.ChatGroups.Add(model);
		await db.SaveChangesAsync(true, ct);

		return new(model.Id);
	}
}