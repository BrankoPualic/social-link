using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Messaging.Data;

#nullable disable

namespace SocialLink.Messaging.Migrations
{
	[DbContext(typeof(EFMessagingDatabaseContext))]
	[Migration("20251111151250_CREATE_ChatGroup_and_ChatGroupUser")]
	/// <inheritdoc />
	public partial class CREATE_ChatGroup_and_ChatGroupUser : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "messaging");

			migrationBuilder.CreateTable(
				name: "ChatGroup",
				schema: "messaging",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastMessageOn = table.Column<DateTime>(type: "datetime2", nullable: true),
					LastMessagePreview = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ChatGroup", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "ChatGroupUser",
				schema: "messaging",
				columns: table => new
				{
					ChatGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastReadMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					IsMuted = table.Column<bool>(type: "bit", nullable: false),
					JoinedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ChatGroupUser", x => new { x.ChatGroupId, x.UserId });
					table.ForeignKey(
						name: "FK_ChatGroupUser_ChatGroup_ChatGroupId",
						column: x => x.ChatGroupId,
						principalSchema: "messaging",
						principalTable: "ChatGroup",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ChatGroupUser_UserId",
				schema: "messaging",
				table: "ChatGroupUser",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ChatGroupUser",
				schema: "messaging");

			migrationBuilder.DropTable(
				name: "ChatGroup",
				schema: "messaging");
		}
	}
}
