using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialMedia.Posts.Data;

#nullable disable

namespace SocialMedia.Posts.Migrations
{
	[DbContext(typeof(PostDatabaseContext))]
	[Migration("20250729215221_ADD_PostMedia_Order")]
	public partial class ADD_PostMedia_Order : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "Order",
				schema: "post",
				table: "PostMedia",
				type: "int",
				nullable: false,
				defaultValue: 0);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Order",
				schema: "post",
				table: "PostMedia");
		}
	}
}