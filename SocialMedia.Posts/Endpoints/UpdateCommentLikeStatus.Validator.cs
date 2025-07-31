using FastEndpoints;
using FluentValidation;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.Posts.Domain;
using SocialMedia.SharedKernel;

namespace SocialMedia.Posts.Endpoints;

internal class UpdateCommentLikeStatusValidator : Validator<CommentLikeDto>
{
	public UpdateCommentLikeStatusValidator()
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