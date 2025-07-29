using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Users.Domain;

namespace SocialMedia.Users.Data.Configurations;

internal class UserMediaConfiguration : IEntityTypeConfiguration<UserMedia>
{
	public void Configure(EntityTypeBuilder<UserMedia> builder)
	{
		builder.HasKey(_ => new { _.UserId, _.BlobId });

		builder.HasOne(_ => _.User)
			.WithMany(_ => _.Media)
			.HasForeignKey(_ => _.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasIndex(_ => _.BlobId);
	}
}