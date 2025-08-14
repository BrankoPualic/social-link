using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel.Data;

namespace SocialLink.Posts.Data.Configurations;

internal class CommentConfiguration : BaseDomainModelConfiguration<Comment, Guid>
{
	protected override void ConfigureDomainModel(EntityTypeBuilder<Comment> builder)
	{
		builder.Property(_ => _.UserId)
			.IsRequired();
		builder.HasIndex(_ => _.UserId);

		builder.HasOne(_ => _.Post)
			.WithMany(_ => _.Comments)
			.HasForeignKey(_ => _.PostId)
			.OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

		builder.HasOne(_ => _.Parent)
			.WithMany(_ => _.Replies)
			.HasForeignKey(_ => _.ParentId)
			.OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

		builder.Property(_ => _.Message)
			.IsRequired()
			.HasMaxLength(500);
	}
}