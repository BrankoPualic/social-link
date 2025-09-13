using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Users.Data;

#nullable disable

namespace SocialLink.Users.Migrations
{
	[DbContext(typeof(UserDatabaseContext))]
	[Migration("20250913123122_UPDATE_UserMedia_Index")]
	public partial class UPDATE_UserMedia_Index : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_UserMedia_UserId_Type",
				schema: "user",
				table: "UserMedia");

			migrationBuilder.CreateIndex(
				name: "IX_UserMedia_UserId_Type_IsActive",
				schema: "user",
				table: "UserMedia",
				columns: new[] { "UserId", "Type", "IsActive" })
				.Annotation("SqlServer:Include", new[] { "BlobId" });
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_UserMedia_UserId_Type_IsActive",
				schema: "user",
				table: "UserMedia");

			migrationBuilder.CreateIndex(
				name: "IX_UserMedia_UserId_Type",
				schema: "user",
				table: "UserMedia",
				columns: new[] { "UserId", "Type" })
				.Annotation("SqlServer:Include", new[] { "BlobId" });
		}
	}
}