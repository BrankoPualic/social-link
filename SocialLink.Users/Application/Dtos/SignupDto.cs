using SocialLink.SharedKernel.Enumerators;
using SocialLink.SharedKernel.Extensions;
using SocialLink.Users.Domain;
using SocialLink.Users.Enumerators;

namespace SocialLink.Users.Application.Dtos;

internal class SignupDto
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Username { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }

	public string RepeatPassword { get; set; }

	public eGender GenderId { get; set; }

	public DateTime? DateOfBirth { get; set; }

	public bool IsPrivate { get; set; }

	public string Biography { get; set; }

	public void ToModel(User model)
	{
		if (model.IsNew)
			model.IsActive = true;

		model.GenerateIdIfNew();

		model.FirstName = FirstName;
		model.LastName = LastName;
		model.Username = Username;
		model.Email = Email;
		model.GenderId = GenderId;
		model.DateOfBirth = DateOfBirth;
		model.IsPrivate = IsPrivate;
		model.Biography = Biography;
		model.Roles = [new UserRole { RoleId = eSystemRole.Member }];
	}
}