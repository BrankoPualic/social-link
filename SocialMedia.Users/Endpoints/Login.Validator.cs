using FastEndpoints;
using FluentValidation;
using SocialMedia.Users.Application.Dtos;

namespace SocialMedia.Users.Endpoints;

internal class LoginValidator : Validator<LoginDto>
{
	public LoginValidator()
	{
		RuleFor(_ => _.Email)
			.NotNull()
			.NotEmpty()
			.WithMessage("Email is required");

		RuleFor(_ => _.Password)
			.NotNull()
			.NotEmpty()
			.WithMessage("Password is required");
	}
}