using FluentValidation;
using MediatR;
using SocialLink.Common.Application;
using SocialLink.Messaging.Application.Dtos;
using SocialLink.Messaging.Domain.Document;
using SocialLink.Messaging.Events;
using SocialLink.SharedKernel;
using SocialLink.Users.Contracts;

namespace SocialLink.Messaging.Application.UseCases.Commands;

internal sealed record CreateMessageCommand(MessageDto Data) : Command<Guid>;

internal class CreateMessageCommandHandler(IMongoMessagingDatabaseContext db, IMediator mediator) : MongoCommandHandler<CreateMessageCommand, Guid>
{
	public override async Task<ResponseWrapper<Guid>> Handle(CreateMessageCommand req, CancellationToken ct)
	{
		var userResult = await mediator.Send(new GetUserContractQuery(req.Data.UserId), ct);
		if (!userResult.IsSuccess)
			return new(userResult.Errors);

		var model = new Message();
		req.Data.ToModel(model);
		model.CreatedOn = DateTime.UtcNow;
		model.LastChangedOn = DateTime.UtcNow;

		model.Id = Guid.NewGuid();
		await db.Messages.InsertOneAsync(model, options: null, cancellationToken: ct);

		req.Data.CreatedOn = model.CreatedOn;
		await mediator.Send(new UpdateChatGroupLastMessageCommand(req.Data), ct);

		await mediator.Publish(new CreateMessageEvent(new Hubs.Message.MessageResponse
		{
			Id = model.Id,
			ChatGroupId = model.ChatGroupId,
			UserId = model.UserId,
			Content = model.Content,
			CreatedOn = model.CreatedOn,
			LastChangedOn = model.LastChangedOn,
			User = userResult.Data
		}), ct);

		return new(model.Id);
	}
}

internal class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
{
	public CreateMessageCommandValidator()
	{
		RuleFor(_ => _.Data.UserId)
			.NotEmpty()
			.NotNull()
			.WithMessage(ResourcesValidation.Required(nameof(Message.UserId)));

		RuleFor(_ => _.Data.ChatGroupId)
			.NotEmpty()
			.NotNull()
			.WithMessage(ResourcesValidation.Required(nameof(Message.ChatGroupId)));

		RuleFor(_ => _.Data.Content)
			.NotEmpty()
			.NotNull()
			.WithMessage(ResourcesValidation.Required(nameof(Message.Content)));
	}
}