using FastEndpoints;
using FluentValidation;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Controllers.AuthValidators;

internal class SignupValidator : Validator<SignupDto>
{
	public SignupValidator()
	{
		RuleFor(_ => _.FirstName)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(User.FirstName)))
			.MaximumLength(20)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.FirstName), 20));

		RuleFor(_ => _.LastName)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(User.LastName)))
			.MaximumLength(50)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.LastName), 50));

		RuleFor(_ => _.Username)
			.Cascade(CascadeMode.Stop)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(User.Username)))
			.MinimumLength(5)
			.WithMessage(ResourcesValidation.MinimumLength(nameof(User.Username), 5))
			.MaximumLength(20)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.Username), 20));

		RuleFor(_ => _.Email)
			.Cascade(CascadeMode.Stop)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(User.Email)))
			.EmailAddress()
			.WithMessage(ResourcesValidation.WrongFormat(nameof(User.Email)))
			.MaximumLength(255)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.LastName), 255));

		RuleFor(_ => _.Password)
			.Cascade(CascadeMode.Stop)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(User.Password)))
			.MinimumLength(8)
			.WithMessage(ResourcesValidation.MinimumLength(nameof(User.Password), 8))
			.MaximumLength(50)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.Password), 50))
			.Matches(@"(?=.*[a-z])")
			.WithMessage("Password lacks 1 lowercase letter")
			.Matches(@"(?=.*[A-Z])")
			.WithMessage("Password lacks 1 uppercase letter")
			.Matches(@"(?=.*\d)")
			.WithMessage("Password lacks 1 digit.")
			.Matches(@"(?=.*[@$!%*?&])")
			.WithMessage("Password lacks 1 special character");

		RuleFor(_ => _.RepeatPassword)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required("Repeat Password"))
			.Equal(_ => _.Password)
			.WithMessage("Repeat password doesn't match password");

		RuleFor(_ => _.GenderId)
			.Cascade(CascadeMode.Stop)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required("Gender"))
			.IsInEnum()
			.WithMessage(ResourcesValidation.InvalidValue("Gender"));

		RuleFor(_ => _.Biography)
			.MaximumLength(150)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.Biography), 150));
	}
}