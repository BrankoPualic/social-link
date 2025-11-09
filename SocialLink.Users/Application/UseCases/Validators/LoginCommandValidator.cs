using FluentValidation;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.UseCases.Commands;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.UseCases.Validators;

internal class LoginCommandValidator : AbstractValidator<LoginCommand>
{
	public LoginCommandValidator()
	{
		RuleFor(_ => _.Data.Email)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(User.Email)));

		RuleFor(_ => _.Data.Password)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(User.Password)));
	}
}
