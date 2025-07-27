using Microsoft.EntityFrameworkCore;
using SocialMedia.Posts.Domain;
using SocialMedia.SharedKernel;
using SocialMedia.SharedKernel.Data;

namespace SocialMedia.Posts.Data;

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

	#endregion DbSets
}