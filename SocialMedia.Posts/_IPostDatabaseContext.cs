using Microsoft.EntityFrameworkCore;
using SocialMedia.Posts.Domain;
using SocialMedia.SharedKernel;

namespace SocialMedia.Posts;

internal interface IPostDatabaseContext : IEFDatabaseContext
{
	DbSet<Post> Posts { get; }

	DbSet<PostLike> PostLikes { get; }

	DbSet<Comment> Comments { get; }

	DbSet<CommentLike> CommentLikes { get; }

	DbSet<PostMedia> Media { get; }
}