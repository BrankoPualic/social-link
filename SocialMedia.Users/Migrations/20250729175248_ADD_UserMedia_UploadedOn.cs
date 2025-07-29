using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialMedia.Users.Data;

#nullable disable

namespace SocialMedia.Users.Migrations
{
	[DbContext(typeof(UserDatabaseContext))]
	[Migration("20250729175248_ADD_UserMedia_UploadedOn")]
	/// <inheritdoc />
	public partial class ADD_UserMedia_UploadedOn : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<DateTime>(
				name: "UploadedOn",
				schema: "user",
				table: "UserMedia",
				type: "datetime2",
				nullable: false);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "UploadedOn",
				schema: "user",
				table: "UserMedia");
		}
	}
}