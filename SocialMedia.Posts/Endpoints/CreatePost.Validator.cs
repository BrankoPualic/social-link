using FastEndpoints;
using FluentValidation;
using SocialMedia.Posts.Domain;
using SocialMedia.SharedKernel;

namespace SocialMedia.Posts.Endpoints;

internal class CreatePostValidator : Validator<CreatePostRequest>
{
	public CreatePostValidator()
	{
		RuleFor(_ => _.Model.UserId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required("UserId"));

		RuleFor(_ => _.Model.Description)
			.MaximumLength(500)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(Post.Description), 500));
	}
}