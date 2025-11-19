using MediatR;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Common.Application;
using SocialLink.Messaging.Application.Dtos;
using SocialLink.Messaging.Domain;
using SocialLink.Messaging.Domain.Relational;
using SocialLink.Messaging.Enumerators;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Enumerators;

namespace SocialLink.Messaging.Application.UseCases.Commands;

internal sealed record CreateGroupConversationCommand(ConversationCreateDto Data, FileInformationDto File) : Command<Guid>;

internal class CreateGroupConversationCommandHandler(IEFMessagingDatabaseContext db, IChatGroupRepository chatGroupRepository, IMediator mediator) : EFCommandHandler<CreateGroupConversationCommand, Guid>(db)
{
	public override async Task<ResponseWrapper<Guid>> Handle(CreateGroupConversationCommand req, CancellationToken ct)
	{
		var data = req.Data;
		var file = req.File;

		if (data is null || data.Users.Count < 2)
			return new(new Error(nameof(ChatGroup), ResourcesValidation.Required("Member")));

		if (string.IsNullOrWhiteSpace(data.Name))
			return new(new Error(nameof(ChatGroup.Name), ResourcesValidation.Required(nameof(ChatGroup.Name))));

		if (file is null)
			return new(new Error("Image", ResourcesValidation.Required("Image")));

		var uploadResult = await mediator.Send(new UploadBlobCommand(new UploadFileDto(file, eBlobType.ChatGroupAvatar)), ct);
		if (!uploadResult.IsSuccess)
			return new(uploadResult.Errors);

		var model = new ChatGroup
		{
			Id = Guid.NewGuid(),
			Name = data.Name,
			IsGroup = true,
			Users = data.Users.Select(userId => new ChatGroupUser
			{
				UserId = userId,
				JoinedOn = DateTime.UtcNow,
				LastChangedOn = DateTime.UtcNow,
			})
			.ToList()
		};

		db.ChatGroups.Add(model);

		chatGroupRepository.CreateMedia(model.Id, uploadResult.Data.BlobId, eChatGroupMedia.ChatGroupAvatar);

		await db.SaveChangesAsync(true, ct);

		await uploadResult.Data.Cleanup.ExecuteAsync();

		return new(model.Id);
	}
}