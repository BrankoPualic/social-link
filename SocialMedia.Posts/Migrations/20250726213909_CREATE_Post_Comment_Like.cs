using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialMedia.Posts.Data;

#nullable disable

namespace SocialMedia.Posts.Migrations
{
	[DbContext(typeof(PostDatabaseContext))]
	[Migration("20250726213909_CREATE_Post_Comment_Like")]
	public partial class CREATE_Post_Comment_Like : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "post");

			migrationBuilder.CreateTable(
				name: "Post",
				schema: "post",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
					AllowComments = table.Column<bool>(type: "bit", nullable: false),
					IsArchived = table.Column<bool>(type: "bit", nullable: false),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Post", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Comment",
				schema: "post",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Comment", x => x.Id);
					table.ForeignKey(
						name: "FK_Comment_Comment_ParentId",
						column: x => x.ParentId,
						principalSchema: "post",
						principalTable: "Comment",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Comment_Post_PostId",
						column: x => x.PostId,
						principalSchema: "post",
						principalTable: "Post",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "PostLike",
				schema: "post",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LikedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PostLike", x => new { x.UserId, x.PostId });
					table.ForeignKey(
						name: "FK_PostLike_Post_PostId",
						column: x => x.PostId,
						principalSchema: "post",
						principalTable: "Post",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "CommentLike",
				schema: "post",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LikedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CommentLike", x => new { x.UserId, x.CommentId });
					table.ForeignKey(
						name: "FK_CommentLike_Comment_CommentId",
						column: x => x.CommentId,
						principalSchema: "post",
						principalTable: "Comment",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Comment_ParentId",
				schema: "post",
				table: "Comment",
				column: "ParentId");

			migrationBuilder.CreateIndex(
				name: "IX_Comment_PostId",
				schema: "post",
				table: "Comment",
				column: "PostId");

			migrationBuilder.CreateIndex(
				name: "IX_Comment_UserId",
				schema: "post",
				table: "Comment",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_CommentLike_CommentId",
				schema: "post",
				table: "CommentLike",
				column: "CommentId");

			migrationBuilder.CreateIndex(
				name: "IX_Post_UserId",
				schema: "post",
				table: "Post",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_PostLike_PostId",
				schema: "post",
				table: "PostLike",
				column: "PostId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "CommentLike",
				schema: "post");

			migrationBuilder.DropTable(
				name: "PostLike",
				schema: "post");

			migrationBuilder.DropTable(
				name: "Comment",
				schema: "post");

			migrationBuilder.DropTable(
				name: "Post",
				schema: "post");
		}
	}
}