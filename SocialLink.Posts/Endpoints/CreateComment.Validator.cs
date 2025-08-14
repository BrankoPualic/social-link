using FastEndpoints;
using FluentValidation;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Endpoints;

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