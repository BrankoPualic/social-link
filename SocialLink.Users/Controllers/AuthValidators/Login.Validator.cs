using FluentValidation;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Controllers.AuthValidators;

internal class LoginValidator : AbstractValidator<LoginDto>
{
	public LoginValidator()
	{
		RuleFor(_ => _.Email)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(User.Email)));

		RuleFor(_ => _.Password)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(User.Password)));
	}
}