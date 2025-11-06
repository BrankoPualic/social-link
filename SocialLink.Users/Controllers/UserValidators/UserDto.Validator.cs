using FastEndpoints;
using FluentValidation;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.Dtos;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Controllers.UserValidators;

internal class UserDtoValidator : Validator<UserDto>
{
	public UserDtoValidator()
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