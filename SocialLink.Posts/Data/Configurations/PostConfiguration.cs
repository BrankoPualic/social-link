using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel.Data;

namespace SocialLink.Posts.Data.Configurations;

internal class PostConfiguration : BaseDomainModelConfiguration<Post, Guid>
{
	protected override void ConfigureDomainModel(EntityTypeBuilder<Post> builder)
	{
		builder.Property(_ => _.UserId)
			.IsRequired();
		builder.HasIndex(_ => _.UserId);

		builder.Property(_ => _.Description)
			.HasMaxLength(500);
	}
}