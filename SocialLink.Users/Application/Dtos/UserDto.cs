using SocialLink.Blobs.Contracts.Dtos;
using SocialLink.Users.Domain;
using System.Linq.Expressions;

namespace SocialLink.Users.Application.Dtos;

internal class UserDto
{
	public Guid Id { get; set; }

	public string Username { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string FullName { get; set; }

	public eGender GenderId { get; set; }

	public bool IsPrivate { get; set; }

	public BlobDto ProfileImage { get; set; }

	public static Expression<Func<User, UserDto>> Projection => _ => new()
	{
		Id = _.Id,
		FirstName = _.FirstName,
		LastName = _.LastName,
		FullName = _.FullName,
		GenderId = _.GenderId,
		IsPrivate = _.IsPrivate,
	};

	public void ToModel(User model)
	{
		model.Username = Username;
		model.FirstName = FirstName;
		model.LastName = LastName;
		model.GenderId = GenderId;
		model.IsPrivate = IsPrivate;
	}
}