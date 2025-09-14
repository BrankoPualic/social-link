using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Users.Data;

#nullable disable

namespace SocialLink.Users.Migrations
{
	[DbContext(typeof(UserDatabaseContext))]
	[Migration("20250914141306_ADD_User_Biography")]
	public partial class ADD_User_Biography : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "Biography",
				schema: "user",
				table: "User",
				type: "nvarchar(150)",
				maxLength: 150,
				nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Biography",
				schema: "user",
				table: "User");
		}
	}
}