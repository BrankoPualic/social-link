using FluentValidation;
using SocialLink.SharedKernel;
using SocialLink.Users.Application.UseCases.Commands;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.UseCases.Validators;
internal class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
	{
		RuleFor(_ => _.Data.FirstName)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(User.FirstName)))
			.MaximumLength(20)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.FirstName), 20));

		RuleFor(_ => _.Data.LastName)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(User.LastName)))
			.MaximumLength(50)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.LastName), 50));

		RuleFor(_ => _.Data.Username)
			.Cascade(CascadeMode.Stop)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(User.Username)))
			.MinimumLength(5)
			.WithMessage(ResourcesValidation.MinimumLength(nameof(User.Username), 5))
			.MaximumLength(20)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.Username), 20));

		RuleFor(_ => _.Data.GenderId)
			.Cascade(CascadeMode.Stop)
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required("Gender"))
			.IsInEnum()
			.WithMessage(ResourcesValidation.InvalidValue("Gender"));

		RuleFor(_ => _.Data.Biography)
			.MaximumLength(150)
			.WithMessage(ResourcesValidation.MaximumLength(nameof(User.Biography), 150));
	}
}