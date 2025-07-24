using Microsoft.EntityFrameworkCore;
using SocialMedia.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Users.Domain;

[PrimaryKey(nameof(UserId), nameof(RoleId))]
internal class UserRole
{
	public Guid UserId { get; set; }

	public eSystemRole RoleId { get; set; }

	[ForeignKey(nameof(UserId))]
	public User User { get; set; }
}