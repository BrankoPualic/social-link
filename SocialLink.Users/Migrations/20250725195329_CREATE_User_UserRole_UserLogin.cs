using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Common.Data;
using SocialLink.Users.Data;

#nullable disable

namespace SocialLink.Users.Migrations
{
	[DbContext(typeof(UserDatabaseContext))]
	[Migration("20250725195329_CREATE_User_UserRole_UserLogin")]
	public partial class CREATE_User_UserRole_UserLogin : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "user");

			migrationBuilder.CreateTable(
				name: "User",
				schema: "user",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
					LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					FullName = table.Column<string>(type: "nvarchar(71)", maxLength: 71, nullable: true, computedColumnSql: "[FirstName] + ' ' + [LastName]"),
					Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
					Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
					Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
					GenderId = table.Column<int>(type: "int", nullable: false),
					DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
					IsPrivate = table.Column<bool>(type: "bit", nullable: false),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					IsLocked = table.Column<bool>(type: "bit", nullable: false),
					CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_User", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "UserLogin",
				schema: "user",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					LoggedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserLogin", x => x.Id);
					table.ForeignKey(
						name: "FK_UserLogin_User_UserId",
						column: x => x.UserId,
						principalSchema: "user",
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "UserRole",
				schema: "user",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					RoleId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
					table.ForeignKey(
						name: "FK_UserRole_User_UserId",
						column: x => x.UserId,
						principalSchema: "user",
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_User_Email",
				schema: "user",
				table: "User",
				column: "Email",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_User_Username",
				schema: "user",
				table: "User",
				column: "Username",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_UserLogin_UserId",
				schema: "user",
				table: "UserLogin",
				column: "UserId");

			migrationBuilder.ExecuteScript("Users", "UserSeeds.sql");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "UserLogin",
				schema: "user");

			migrationBuilder.DropTable(
				name: "UserRole",
				schema: "user");

			migrationBuilder.DropTable(
				name: "User",
				schema: "user");
		}
	}
}