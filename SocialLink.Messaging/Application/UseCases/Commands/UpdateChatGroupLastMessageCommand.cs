using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Application;
using SocialLink.Messaging.Application.Dtos;
using SocialLink.SharedKernel;

namespace SocialLink.Messaging.Application.UseCases.Commands;

internal sealed record UpdateChatGroupLastMessageCommand(MessageDto Data) : Command;

internal class UpdateChatGroupLastMessageCommandHandler(IEFMessagingDatabaseContext db) : EFCommandHandler<UpdateChatGroupLastMessageCommand>(db)
{
	public override async Task<ResponseWrapper> Handle(UpdateChatGroupLastMessageCommand req, CancellationToken ct)
	{
		var model = await db.ChatGroups.FirstOrDefaultAsync(_ => _.Id == req.Data.ChatGroupId, ct);
		if (model is null)
			return new();

		model.LastMessageOn = req.Data.CreatedOn;

		if (req.Data.Type == Enumerators.eMessageType.Audio)
		{
			model.LastMessagePreview = "Voice message";
		}
		else
		{
			model.LastMessagePreview = req.Data.Content.Length > 97
				? req.Data.Content[..97] + "..."
				: req.Data.Content;
		}

		await db.SaveChangesAsync(true, ct);

		return new();
	}
}