using FluentValidation;
using SocialLink.Posts.Application.UseCases.Commands;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application.UseCases.Validators;
internal class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
	public CreateCommentCommandValidator()
	{
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
