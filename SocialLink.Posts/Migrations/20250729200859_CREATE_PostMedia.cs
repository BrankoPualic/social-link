using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Posts.Data;


#nullable disable

namespace SocialLink.Posts.Migrations
{
	[DbContext(typeof(PostDatabaseContext))]
	[Migration("20250729200859_CREATE_PostMedia")]
	/// <inheritdoc />
	public partial class CREATE_PostMedia : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "PostMedia",
				schema: "post",
				columns: table => new
				{
					PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					BlobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					UploadedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PostMedia", x => new { x.PostId, x.BlobId });
					table.ForeignKey(
						name: "FK_PostMedia_Post_PostId",
						column: x => x.PostId,
						principalSchema: "post",
						principalTable: "Post",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_PostMedia_BlobId",
				schema: "post",
				table: "PostMedia",
				column: "BlobId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "PostMedia",
				schema: "post");
		}
	}
}