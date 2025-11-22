using FluentValidation;
using MediatR;
using SocialLink.Blobs.Contracts.Commands;
using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Common.Application;
using SocialLink.Messaging.Application.Dtos;
using SocialLink.Messaging.Domain.Document;
using SocialLink.Messaging.Enumerators;
using SocialLink.Messaging.Events;
using SocialLink.SharedKernel;
using SocialLink.SharedKernel.Enumerators;
using SocialLink.SharedKernel.Extensions;
using SocialLink.Users.Contracts;

namespace SocialLink.Messaging.Application.UseCases.Commands;
internal sealed record CreateAudioMessageCommand(Guid ChatGroupId, FileInformationDto File) : Command<Guid>;

internal class CreateAudioMessageCommandHandler(IMongoMessagingDatabaseContext db, IMediator mediator) : MongoCommandHandler<CreateAudioMessageCommand, Guid>
{
	public override async Task<ResponseWrapper<Guid>> Handle(CreateAudioMessageCommand req, CancellationToken ct)
	{
		var userResult = await mediator.Send(new GetUserContractQuery(db.CurrentUser.Id), ct);
		if (!userResult.IsSuccess)
			return new(userResult.Errors);

		var blobResult = await mediator.Send(new UploadBlobCommand(new UploadFileDto(req.File, eBlobType.AudioMessage)), ct);
		if (!blobResult.IsSuccess)
			return new(blobResult.Errors);

		var model = new Message
		{
			Id = Guid.NewGuid(),
			ChatGroupId = req.ChatGroupId,
			UserId = db.CurrentUser.Id,
			Type = eMessageType.Audio,
			Content = new { BlobId = blobResult.Data.BlobId }.SerializeJsonObject(),
			CreatedOn = DateTime.UtcNow,
			LastChangedOn = DateTime.UtcNow,
		};

		await db.Messages.InsertOneAsync(model, options: null, cancellationToken: ct);

		var data = MessageDto.Projection.Compile()(model);
		await mediator.Send(new UpdateChatGroupLastMessageCommand(data), ct);

		await mediator.Publish(new CreateMessageEvent(new Hubs.Message.MessageResponse
		{
			Id = model.Id,
			ChatGroupId = model.ChatGroupId,
			UserId = model.UserId,
			Type = model.Type,
			Content = model.Content,
			CreatedOn = model.CreatedOn,
			LastChangedOn = model.LastChangedOn,
			User = userResult.Data,
		}), ct);

		return new(model.Id);
	}
}

internal class CreateAudioMessageCommandValidator : AbstractValidator<CreateAudioMessageCommand>
{
	public CreateAudioMessageCommandValidator()
	{
		RuleFor(_ => _.ChatGroupId)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Message.ChatGroupId)));

		RuleFor(_ => _.File.Buffer)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required("File"));
	}
}