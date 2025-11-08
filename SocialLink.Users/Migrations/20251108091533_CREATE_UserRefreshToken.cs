using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Users.Data;

#nullable disable

namespace SocialLink.Users.Migrations
{
	[DbContext(typeof(UserDatabaseContext))]
	[Migration("20251108091533_CREATE_UserRefreshToken")]
	/// <inheritdoc />
	public partial class CREATE_UserRefreshToken : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "UserRefreshToken",
				schema: "user",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Token = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
					TokenExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserRefreshToken", x => x.Id);
					table.ForeignKey(
						name: "FK_UserRefreshToken_User_UserId",
						column: x => x.UserId,
						principalSchema: "user",
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_UserRefreshToken_Token",
				schema: "user",
				table: "UserRefreshToken",
				column: "Token",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_UserRefreshToken_UserId",
				schema: "user",
				table: "UserRefreshToken",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "UserRefreshToken",
				schema: "user");
		}
	}
}
