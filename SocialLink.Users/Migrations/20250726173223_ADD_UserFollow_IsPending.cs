using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Users.Data;


#nullable disable

namespace SocialLink.Users.Migrations
{
	[DbContext(typeof(UserDatabaseContext))]
	[Migration("20250726173223_ADD_UserFollow_IsPending")]
	/// <inheritdoc />
	public partial class ADD_UserFollow_IsPending : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "IsPending",
				schema: "user",
				table: "UserFollow",
				type: "bit",
				nullable: false,
				defaultValue: false);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "IsPending",
				schema: "user",
				table: "UserFollow");
		}
	}
}