using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.Posts.Domain;

namespace SocialLink.Posts.Data.Configurations;

internal class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
{
	public void Configure(EntityTypeBuilder<PostLike> builder)
	{
		builder.HasKey(_ => new { _.UserId, _.PostId });

		builder.HasOne(_ => _.Post)
			.WithMany()
			.HasForeignKey(_ => _.PostId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}