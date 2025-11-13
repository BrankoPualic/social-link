using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Messaging.Data;

#nullable disable

namespace SocialLink.Messaging.Migrations
{
	[DbContext(typeof(EFMessagingDatabaseContext))]
	[Migration("20251113184739_UPDATE_ChatGroupUser_LastReadMessageId")]
	/// <inheritdoc />
	public partial class UPDATE_ChatGroupUser_LastReadMessageId : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<Guid>(
				name: "LastReadMessageId",
				schema: "messaging",
				table: "ChatGroupUser",
				type: "uniqueidentifier",
				nullable: true,
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<Guid>(
				name: "LastReadMessageId",
				schema: "messaging",
				table: "ChatGroupUser",
				type: "uniqueidentifier",
				nullable: false,
				defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier",
				oldNullable: true);
		}
	}
}
