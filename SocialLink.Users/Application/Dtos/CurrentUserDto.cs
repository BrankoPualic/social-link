using SocialLink.SharedKernel;
using SocialLink.Users.Domain;
using SocialLink.Users.Enumerators;
using System.Linq.Expressions;

namespace SocialLink.Users.Application.Dtos;

internal class CurrentUserDto
{
	public Guid Id { get; set; }

	public string Username { get; set; }

	public string FullName { get; set; }

	public eGender GenderId { get; set; }

	public bool IsPrivate { get; set; }

	public List<LookupDto> Roles { get; set; } = [];

	public static Expression<Func<User, CurrentUserDto>> Projection => _ => new()
	{
		Id = _.Id,
		Username = _.Username,
		FullName = _.FullName,
		GenderId = _.GenderId,
		IsPrivate = _.IsPrivate,
		Roles = _.Roles
					.ToList()
					.Select(_ => LookupDto.EnumProjection(_.RoleId))
					.ToList()
	};
}