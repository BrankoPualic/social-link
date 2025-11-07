using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.Common.Data;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Data.Configurations;

internal class UserLoginConfiguration : BaseDomainModelConfiguration<UserLogin, Guid>
{
	protected override void ConfigureDomainModel(EntityTypeBuilder<UserLogin> builder)
	{
		builder.HasOne(_ => _.User)
			.WithMany(_ => _.Logins)
			.HasForeignKey(_ => _.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}