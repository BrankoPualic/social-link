using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialMedia.Users.Data;

#nullable disable

namespace SocialMedia.Users.Migrations
{
	[DbContext(typeof(UserDatabaseContext))]
	[Migration("20250726130056_CREATE_UserFollow")]
	public partial class CREATE_UserFollow : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "UserFollow",
				schema: "user",
				columns: table => new
				{
					FollowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					FollowingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					FollowDate = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserFollow", x => new { x.FollowerId, x.FollowingId });
					table.ForeignKey(
						name: "FK_UserFollow_User_FollowerId",
						column: x => x.FollowerId,
						principalSchema: "user",
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_UserFollow_User_FollowingId",
						column: x => x.FollowingId,
						principalSchema: "user",
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_UserFollow_FollowingId",
				schema: "user",
				table: "UserFollow",
				column: "FollowingId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "UserFollow",
				schema: "user");
		}
	}
}