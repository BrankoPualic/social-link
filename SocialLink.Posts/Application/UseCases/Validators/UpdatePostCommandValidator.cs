using FluentValidation;
using SocialLink.Posts.Application.UseCases.Commands;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Application.UseCases.Validators;

internal class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
	public UpdatePostCommandValidator()
	{
		RuleFor(_ => _.Data.Id)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Post.Id)));

		RuleFor(_ => _.Data.UserId)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Post.UserId)));

		RuleFor(_ => _.Data.Description)
			.MaximumLength(500)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(Post.Description), 500));
	}
}