using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Messaging.Data;

#nullable disable

namespace SocialLink.Messaging.Migrations
{
	[DbContext(typeof(EFMessagingDatabaseContext))]
	[Migration("20251116210043_CREATE_ChatGroupMedia")]
	/// <inheritdoc />
	public partial class CREATE_ChatGroupMedia : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "ChatGroupMedia",
				schema: "messaging",
				columns: table => new
				{
					ChatGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					BlobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Type = table.Column<int>(type: "int", nullable: true),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					UploadedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ChatGroupMedia", x => new { x.ChatGroupId, x.BlobId });
					table.ForeignKey(
						name: "FK_ChatGroupMedia_ChatGroup_ChatGroupId",
						column: x => x.ChatGroupId,
						principalSchema: "messaging",
						principalTable: "ChatGroup",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ChatGroupMedia_ChatGroupId_Type_IsActive",
				schema: "messaging",
				table: "ChatGroupMedia",
				columns: new[] { "ChatGroupId", "Type", "IsActive" })
				.Annotation("SqlServer:Include", new[] { "BlobId" });
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ChatGroupMedia",
				schema: "messaging");
		}
	}
}
