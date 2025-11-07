using Microsoft.EntityFrameworkCore;
using SocialLink.Common.Data;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel.Domain;

namespace SocialLink.Posts.Data;

internal sealed class PostDatabaseContext : EFDatabaseContext, IPostDatabaseContext
{
	public PostDatabaseContext(IIdentityUser currentUser) : base(currentUser)
	{ }

	public PostDatabaseContext(DbContextOptions<PostDatabaseContext> options, IIdentityUser currentUser) : base(options, currentUser)
	{ }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("post");

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostDatabaseContext).Assembly);

		foreach (var entity in modelBuilder.Model.GetEntityTypes())
		{
			entity.SetTableName(entity.DisplayName());
		}
	}

	#region DbSets

	public DbSet<Post> Posts { get; set; }

	public DbSet<PostLike> PostLikes { get; set; }

	public DbSet<Comment> Comments { get; set; }

	public DbSet<CommentLike> CommentLikes { get; set; }

	public DbSet<PostMedia> Media { get; set; }

	#endregion DbSets
}