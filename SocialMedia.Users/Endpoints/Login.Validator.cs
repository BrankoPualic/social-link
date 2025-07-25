using FastEndpoints;
using FluentValidation;
using SocialMedia.SharedKernel;
using SocialMedia.Users.Application.Dtos;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Endpoints;

internal class LoginValidator : Validator<LoginDto>
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