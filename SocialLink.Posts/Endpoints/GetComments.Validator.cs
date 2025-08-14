using FastEndpoints;
using FluentValidation;
using SocialLink.Posts.Application;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Endpoints;

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