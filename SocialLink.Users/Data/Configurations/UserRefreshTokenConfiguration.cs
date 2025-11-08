using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.Common.Data;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Data.Configurations;

internal class UserRefreshTokenConfiguration : BaseDomainModelConfiguration<UserRefreshToken, Guid>
{
	protected override void ConfigureDomainModel(EntityTypeBuilder<UserRefreshToken> builder)
	{
		builder.Property(_ => _.Token)
			.IsRequired()
			.HasMaxLength(200);

		builder.HasIndex(_ => _.Token).IsUnique();

		builder.HasOne(_ => _.User)
			.WithMany(_ => _.RefreshTokens)
			.HasForeignKey(_ => _.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}