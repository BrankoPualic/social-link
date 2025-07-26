using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialMedia.Notifications.Data;

#nullable disable

namespace SocialMedia.Notifications.Migrations
{
	[DbContext(typeof(NotificationDatabaseContext))]
	[Migration("20250726154840_CREATE_Notification")]
	public partial class CREATE_Notification : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "notification");

			migrationBuilder.CreateTable(
				name: "Notification",
				schema: "notification",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					TypeId = table.Column<int>(type: "int", nullable: false),
					Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
					Message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
					Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
					IsRead = table.Column<bool>(type: "bit", nullable: false),
					IsSent = table.Column<bool>(type: "bit", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Notification", x => x.Id);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Notification_UserId",
				schema: "notification",
				table: "Notification",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Notification",
				schema: "notification");
		}
	}
}