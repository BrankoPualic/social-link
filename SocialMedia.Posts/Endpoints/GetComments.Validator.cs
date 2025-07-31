using FastEndpoints;
using FluentValidation;
using SocialMedia.Posts.Application;
using SocialMedia.Posts.Domain;
using SocialMedia.SharedKernel;

namespace SocialMedia.Posts.Endpoints;

internal class GetCommentsValidator : Validator<CommentSearch>
{
	public GetCommentsValidator()
	{
		RuleFor(_ => _.PostId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Comment.Post)));
	}
}