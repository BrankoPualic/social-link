using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Data.Configurations;

internal class UserFollowConfiguration : IEntityTypeConfiguration<UserFollow>
{
	public void Configure(EntityTypeBuilder<UserFollow> builder)
	{
		builder.HasKey(_ => new { _.FollowerId, _.FollowingId });

		builder.HasOne(_ => _.Follower)
			.WithMany(_ => _.Following)
			.HasForeignKey(_ => _.FollowerId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(_ => _.Following)
			.WithMany(_ => _.Followers)
			.HasForeignKey(_ => _.FollowingId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}