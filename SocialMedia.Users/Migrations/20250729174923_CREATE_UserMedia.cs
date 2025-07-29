using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialMedia.Users.Data;

#nullable disable

namespace SocialMedia.Users.Migrations
{
	[DbContext(typeof(UserDatabaseContext))]
	[Migration("20250729174923_CREATE_UserMedia")]
	/// <inheritdoc />
	public partial class CREATE_UserMedia : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "UserMedia",
				schema: "user",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					BlobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					IsActive = table.Column<bool>(type: "bit", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserMedia", x => new { x.UserId, x.BlobId });
					table.ForeignKey(
						name: "FK_UserMedia_User_UserId",
						column: x => x.UserId,
						principalSchema: "user",
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_UserMedia_BlobId",
				schema: "user",
				table: "UserMedia",
				column: "BlobId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "UserMedia",
				schema: "user");
		}
	}
}