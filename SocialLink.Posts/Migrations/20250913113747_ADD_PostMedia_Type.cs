using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Posts.Data;

#nullable disable

namespace SocialLink.Posts.Migrations
{
	[DbContext(typeof(PostDatabaseContext))]
	[Migration("20250913113747_ADD_PostMedia_Type")]
	public partial class ADD_PostMedia_Type : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_PostMedia_BlobId",
				schema: "post",
				table: "PostMedia");

			migrationBuilder.AddColumn<int>(
				name: "Type",
				schema: "post",
				table: "PostMedia",
				type: "int",
				nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Type",
				schema: "post",
				table: "PostMedia");

			migrationBuilder.CreateIndex(
				name: "IX_PostMedia_BlobId",
				schema: "post",
				table: "PostMedia",
				column: "BlobId");
		}
	}
}