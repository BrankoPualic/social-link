using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Users.Data;

#nullable disable

namespace SocialLink.Users.Migrations
{
	[DbContext(typeof(UserDatabaseContext))]
	[Migration("20250913111831_ADD_UserMedia_Type")]
	public partial class ADD_UserMedia_Type : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_UserMedia_BlobId",
				schema: "user",
				table: "UserMedia");

			migrationBuilder.AddColumn<int>(
				name: "Type",
				schema: "user",
				table: "UserMedia",
				type: "int",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_UserMedia_UserId_Type",
				schema: "user",
				table: "UserMedia",
				columns: new[] { "UserId", "Type" })
				.Annotation("SqlServer:Include", new[] { "BlobId" });
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_UserMedia_UserId_Type",
				schema: "user",
				table: "UserMedia");

			migrationBuilder.DropColumn(
				name: "Type",
				schema: "user",
				table: "UserMedia");

			migrationBuilder.CreateIndex(
				name: "IX_UserMedia_BlobId",
				schema: "user",
				table: "UserMedia",
				column: "BlobId");
		}
	}
}