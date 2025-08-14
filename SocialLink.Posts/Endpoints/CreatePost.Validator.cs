using FastEndpoints;
using FluentValidation;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Endpoints;

internal class CreatePostValidator : Validator<CreatePostRequest>
{
	public CreatePostValidator()
	{
		RuleFor(_ => _.Model.UserId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Post.UserId)));

		RuleFor(_ => _.Model.Description)
			.MaximumLength(500)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(Post.Description), 500));
	}
}