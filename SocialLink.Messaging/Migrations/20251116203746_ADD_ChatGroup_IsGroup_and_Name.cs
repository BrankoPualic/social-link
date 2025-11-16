using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Messaging.Data;

#nullable disable

namespace SocialLink.Messaging.Migrations
{
	[DbContext(typeof(EFMessagingDatabaseContext))]
	[Migration("20251116203746_ADD_ChatGroup_IsGroup_and_Name")]
	/// <inheritdoc />
	public partial class ADD_ChatGroup_IsGroup_and_Name : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "IsGroup",
				schema: "messaging",
				table: "ChatGroup",
				type: "bit",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "Name",
				schema: "messaging",
				table: "ChatGroup",
				type: "nvarchar(20)",
				maxLength: 20,
				nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "IsGroup",
				schema: "messaging",
				table: "ChatGroup");

			migrationBuilder.DropColumn(
				name: "Name",
				schema: "messaging",
				table: "ChatGroup");
		}
	}
}
