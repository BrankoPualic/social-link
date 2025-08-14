using FastEndpoints;
using FluentValidation;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Users.Endpoints;

internal class SignupValidator : Validator<SignupDto>
{
	public SignupValidator()
	{
		RuleFor(_ => _.FirstName)
			.NotEmpty()
			.NotNull()
			.WithMessage(ResourcesValidation.Required(nameof(User.FirstName)))
			.MaximumLength(20)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.FirstName), 20));

		RuleFor(_ => _.LastName)
			.NotEmpty()
			.NotNull()
			.WithMessage(ResourcesValidation.Required(nameof(User.LastName)))
			.MaximumLength(50)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.LastName), 50));

		RuleFor(_ => _.Username)
			.NotEmpty()
			.NotNull()
			.WithMessage(ResourcesValidation.Required(nameof(User.Username)))
			.MinimumLength(5)
			.WithMessage(ResourcesValidation.MinimumLength(nameof(User.Username), 5))
			.MaximumLength(20)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.Username), 20));

		RuleFor(_ => _.Email)
			.NotEmpty()
			.NotNull()
			.WithMessage(ResourcesValidation.Required(nameof(User.Email)))
			.EmailAddress()
			.WithMessage(ResourcesValidation.WrongFormat(nameof(User.Email)))
			.MaximumLength(255)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.LastName), 255));

		RuleFor(_ => _.Password)
			.NotEmpty()
			.NotNull()
			.WithMessage(ResourcesValidation.Required(nameof(User.Password)))
			.MinimumLength(8)
			.WithMessage(ResourcesValidation.MinimumLength(nameof(User.Password), 8))
			.MaximumLength(50)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.Password), 50))
			.Matches(@"(?=.*[a-z])")
			.WithMessage("Password lacks 1 lowercase letter.")
			.Matches(@"(?=.*[A-Z])")
			.WithMessage("Password lacks 1 uppercase letter.")
			.Matches(@"(?=.*\d)")
			.WithMessage("Password lacks 1 digit.")
			.Matches(@"(?=.*[@$!%*?&])")
			.WithMessage("Password lacks 1 special character.");

		RuleFor(_ => _.RepeatPassword)
			.NotEmpty()
			.NotNull()
			.WithMessage(ResourcesValidation.Required("Repeat Password"))
			.Equal(_ => _.Password)
			.WithMessage("Repeat password doesn't match password.");

		RuleFor(_ => _.GenderId)
			.NotEmpty()
			.NotNull()
			.WithMessage(ResourcesValidation.Required("Gender"))
			.IsInEnum()
			.WithMessage(ResourcesValidation.InvalidValue("Gender"));
	}
}