using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.Common.Data;
using SocialLink.Messaging.Domain.Relational;

namespace SocialLink.Messaging.Data.Configurations;

internal class ChatGroupConfiguration : BaseDomainModelConfiguration<ChatGroup, Guid>
{
	protected override void ConfigureDomainModel(EntityTypeBuilder<ChatGroup> builder)
	{
		builder.Property(_ => _.LastMessagePreview)
			.HasMaxLength(100);

		builder.Property(_ => _.Name)
			.HasMaxLength(20);
	}
}