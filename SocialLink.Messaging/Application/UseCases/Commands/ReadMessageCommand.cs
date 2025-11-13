using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.Messaging.Application.Dtos;
using SocialLink.SharedKernel;

namespace SocialLink.Messaging.Application.UseCases.Commands;

internal sealed record ReadMessageCommand(ReadMessageDto Data) : Command;

internal class ReadUnreadMessagesCommandHandler(IEFMessagingDatabaseContext db) : EFCommandHandler<ReadMessageCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(ReadMessageCommand req, CancellationToken ct)
	{
		var model = await db.ChatGroupUsers
			.Where(_ => _.UserId == req.Data.UserId)
			.Where(_ => _.ChatGroupId == req.Data.ChatGroupId)
			.FirstOrDefaultAsync(ct);

		if (model is null)
			return new();

		model.LastReadMessageId = req.Data.LastMessageId;

		await db.SaveChangesAsync(false, ct);

		return new();
	}
}