using FastEndpoints;
using FluentValidation;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Endpoints;

internal class UpdatePostValidator : Validator<PostEditDto>
{
	public UpdatePostValidator()
	{
		RuleFor(_ => _.Id)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Post.Id)));

		RuleFor(_ => _.UserId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Post.UserId)));

		RuleFor(_ => _.Description)
			.MaximumLength(500)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(Post.Description), 500));
	}
}