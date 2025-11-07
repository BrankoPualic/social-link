using FluentValidation;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Controllers.PostValidators;

internal class PostCreateDtoValidator : AbstractValidator<PostCreateDto>
{
	public PostCreateDtoValidator()
	{
		RuleFor(_ => _.UserId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Post.UserId)));

		RuleFor(_ => _.Description)
			.MaximumLength(500)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(Post.Description), 500));
	}
}