using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Notifications.Domain;
using SocialMedia.SharedKernel.Data;

namespace SocialMedia.Notifications.Data;

internal class NotificationConfiguration : BaseDomainModelConfiguration<Notification, Guid>
{
	protected override void ConfigureDomainModel(EntityTypeBuilder<Notification> builder)
	{
		builder.Property(_ => _.Title)
			.HasMaxLength(50);

		builder.Property(_ => _.Message)
			.HasMaxLength(255);

		builder.Property(_ => _.UserId)
			.IsRequired();
		builder.HasIndex(_ => _.UserId);
	}
}