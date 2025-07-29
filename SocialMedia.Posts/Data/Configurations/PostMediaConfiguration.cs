using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Posts.Domain;

namespace SocialMedia.Posts.Data.Configurations;

internal class PostMediaConfiguration : IEntityTypeConfiguration<PostMedia>
{
	public void Configure(EntityTypeBuilder<PostMedia> builder)
	{
		builder.HasKey(_ => new { _.PostId, _.BlobId });

		builder.HasOne(_ => _.Post)
			.WithMany(_ => _.Media)
			.HasForeignKey(_ => _.PostId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasIndex(_ => _.BlobId);
	}
}