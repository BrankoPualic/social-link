using FluentValidation;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Controllers.CommentValidators;

internal class CommentLikeDtoValidator : AbstractValidator<CommentLikeDto>
{
	public CommentLikeDtoValidator()
	{
		RuleFor(_ => _.CommentId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(CommentLike.CommentId)));

		RuleFor(_ => _.UserId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(CommentLike.UserId)));
	}
}