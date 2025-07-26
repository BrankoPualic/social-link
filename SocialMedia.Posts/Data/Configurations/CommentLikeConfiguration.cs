using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Posts.Domain;

namespace SocialMedia.Posts.Data.Configurations;

internal class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
{
	public void Configure(EntityTypeBuilder<CommentLike> builder)
	{
		builder.HasKey(_ => new { _.UserId, _.CommentId });

		builder.HasOne(_ => _.Comment)
			.WithMany()
			.HasForeignKey(_ => _.CommentId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}