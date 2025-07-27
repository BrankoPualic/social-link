using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialMedia.Users.Data;

#nullable disable

namespace SocialMedia.Users.Migrations
{
	[DbContext(typeof(UserDatabaseContext))]
	[Migration("20250727185235_CREATE_NotificationPreference")]
	/// <inheritdoc />
	public partial class CREATE_NotificationPreference : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "NotificationPreference",
				schema: "user",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					NotificationTypeId = table.Column<int>(type: "int", nullable: false),
					IsMuted = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_NotificationPreference", x => x.Id);
					table.ForeignKey(
						name: "FK_NotificationPreference_User_UserId",
						column: x => x.UserId,
						principalSchema: "user",
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_NotificationPreference_UserId",
				schema: "user",
				table: "NotificationPreference",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "NotificationPreference",
				schema: "user");
		}
	}
}