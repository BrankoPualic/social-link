using FastEndpoints;
using FluentValidation;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.Posts.Domain;
using SocialMedia.SharedKernel;

namespace SocialMedia.Posts.Endpoints;

internal class CreateCommentValidator : Validator<CommentEditDto>
{
	public CreateCommentValidator()
	{
		RuleFor(_ => _.UserId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Comment.UserId)));

		RuleFor(_ => _.PostId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Comment.PostId)));

		RuleFor(_ => _.Message)
			.MaximumLength(500)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(Comment.Message), 500));
	}
}