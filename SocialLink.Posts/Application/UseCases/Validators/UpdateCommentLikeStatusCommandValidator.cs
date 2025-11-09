using FluentValidation;
using SocialLink.Posts.Application.UseCases.Commands;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application.UseCases.Validators;

internal class UpdateCommentLikeStatusCommandValidator : AbstractValidator<UpdateCommentLikeStatusCommand>
{
	public UpdateCommentLikeStatusCommandValidator()
	{
		RuleFor(_ => _.Data.CommentId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(CommentLike.CommentId)));

		RuleFor(_ => _.Data.UserId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(CommentLike.UserId)));
	}
}