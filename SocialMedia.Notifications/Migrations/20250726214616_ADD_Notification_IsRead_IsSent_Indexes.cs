using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialMedia.Notifications.Data;

#nullable disable

namespace SocialMedia.Notifications.Migrations
{
	[DbContext(typeof(NotificationDatabaseContext))]
	[Migration("20250726214616_ADD_Notification_IsRead_IsSent_Indexes")]
	/// <inheritdoc />
	public partial class ADD_Notification_IsRead_IsSent_Indexes : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateIndex(
				name: "IX_Notification_IsRead",
				schema: "notification",
				table: "Notification",
				column: "IsRead");

			migrationBuilder.CreateIndex(
				name: "IX_Notification_IsSent",
				schema: "notification",
				table: "Notification",
				column: "IsSent");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_Notification_IsRead",
				schema: "notification",
				table: "Notification");

			migrationBuilder.DropIndex(
				name: "IX_Notification_IsSent",
				schema: "notification",
				table: "Notification");
		}
	}
}