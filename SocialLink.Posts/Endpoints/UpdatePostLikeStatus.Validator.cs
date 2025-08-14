using FastEndpoints;
using FluentValidation;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Endpoints;

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