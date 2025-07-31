using FastEndpoints;
using FluentValidation;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.Posts.Domain;
using SocialMedia.SharedKernel;

namespace SocialMedia.Posts.Endpoints;

internal class UpdatePostLikeStatusValidator : Validator<PostLikeDto>
{
	public UpdatePostLikeStatusValidator()
	{
		RuleFor(_ => _.PostId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(PostLike.PostId)));

		RuleFor(_ => _.UserId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(CommentLike.UserId)));
	}
}