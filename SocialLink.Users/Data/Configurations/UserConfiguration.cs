using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.SharedKernel.Data;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Data.Configurations;

internal class UserConfiguration : BaseDomainModelConfiguration<User, Guid>
{
	protected override void ConfigureDomainModel(EntityTypeBuilder<User> builder)
	{
		builder.Property(_ => _.FirstName)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(_ => _.LastName)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(_ => _.FullName)
			.HasMaxLength(71)
			.HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

		builder.Property(_ => _.Username)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(_ => _.Email)
			.IsRequired()
			.HasMaxLength(255);

		builder.HasIndex(_ => _.Email)
			.IsUnique();
		builder.HasIndex(_ => _.Username)
			.IsUnique();

		builder.Property(_ => _.Biography)
			.HasMaxLength(150);
	}
}