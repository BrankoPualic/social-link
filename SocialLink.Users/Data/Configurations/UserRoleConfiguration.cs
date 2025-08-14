using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Data.Configurations;

internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
	public void Configure(EntityTypeBuilder<UserRole> builder)
	{
		builder.HasKey(_ => new { _.UserId, _.RoleId });

		builder.HasOne(_ => _.User)
			.WithMany(_ => _.Roles)
			.HasForeignKey(_ => _.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}