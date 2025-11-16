using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.Messaging.Domain.Relational;

namespace SocialLink.Messaging.Data.Configurations;

internal class ChatGroupMediaConfiguration : IEntityTypeConfiguration<ChatGroupMedia>
{
	public void Configure(EntityTypeBuilder<ChatGroupMedia> builder)
	{
		builder.HasKey(_ => new { _.ChatGroupId, _.BlobId });

		builder.HasOne(_ => _.ChatGroup)
			.WithMany(_ => _.Media)
			.HasForeignKey(_ => _.ChatGroupId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasIndex(_ => new { _.ChatGroupId, _.Type, _.IsActive })
			.IncludeProperties(_ => _.BlobId);
	}
}