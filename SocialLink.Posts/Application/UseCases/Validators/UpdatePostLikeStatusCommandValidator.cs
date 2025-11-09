using FluentValidation;
using SocialLink.Posts.Application.UseCases.Commands;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application.UseCases.Validators;

internal class UpdatePostLikeStatusCommandValidator : AbstractValidator<UpdatePostLikeStatusCommand>
{
	public UpdatePostLikeStatusCommandValidator()
	{
		RuleFor(_ => _.Data.PostId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(PostLike.PostId)));

		RuleFor(_ => _.Data.UserId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(CommentLike.UserId)));
	}
}