using FluentValidation;
using SocialLink.Posts.Application.UseCases.Commands;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application.UseCases.Validators;

internal class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
	public UpdateCommentCommandValidator()
	{
		RuleFor(_ => _.Data.Id)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Comment.Id)));

		RuleFor(_ => _.Data.UserId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Comment.UserId)));

		RuleFor(_ => _.Data.PostId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Comment.PostId)));

		RuleFor(_ => _.Data.Message)
			.MaximumLength(500)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(Comment.Message), 500));
	}
}