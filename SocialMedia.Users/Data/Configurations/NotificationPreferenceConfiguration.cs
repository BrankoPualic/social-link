using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.SharedKernel.Data;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Data.Configurations;

internal class NotificationPreferenceConfiguration : BaseDomainModelConfiguration<NotificationPreference, Guid>
{
	protected override void ConfigureDomainModel(EntityTypeBuilder<NotificationPreference> builder)
	{
		builder.HasOne(_ => _.User)
			.WithMany(_ => _.NotificationPreferences)
			.HasForeignKey(_ => _.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.Property(_ => _.NotificationTypeId)
			.IsRequired();
	}
}