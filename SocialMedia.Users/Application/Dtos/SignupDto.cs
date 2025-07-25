using SocialMedia.SharedKernel;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Application.Dtos;

internal class SignupDto
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Username { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }

	public string RepeatPassword { get; set; }

	public eGender GenderId { get; set; }

	public bool IsPrivate { get; set; }

	public void ToModel(User model)
	{
		model.GenerateIdIfNew();

		if (model.IsNew)
			model.IsActive = true;

		model.FirstName = FirstName;
		model.LastName = LastName;
		model.Username = Username;
		model.Email = Email;
		model.GenderId = GenderId;
		model.IsPrivate = IsPrivate;
		model.Roles = [new UserRole { RoleId = eSystemRole.Member }];
	}
}