using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.Messaging.Domain.Relational;

namespace SocialLink.Messaging.Data.Configurations;

internal class ChatGroupUserConfiguration : IEntityTypeConfiguration<ChatGroupUser>
{
	public void Configure(EntityTypeBuilder<ChatGroupUser> builder)
	{
		builder.HasKey(_ => new { _.ChatGroupId, _.UserId });

		builder.HasIndex(_ => _.UserId);

		builder.HasOne(_ => _.ChatGroup)
			.WithMany(_ => _.Users)
			.HasForeignKey(_ => _.ChatGroupId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}