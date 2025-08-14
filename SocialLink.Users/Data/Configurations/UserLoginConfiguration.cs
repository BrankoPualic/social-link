using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.Users.Domain;
using SocialLink.SharedKernel.Data;

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