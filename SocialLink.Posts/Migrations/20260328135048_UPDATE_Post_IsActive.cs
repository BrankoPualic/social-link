using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Posts.Data;

#nullable disable

namespace SocialLink.Posts.Migrations
{
	[DbContext(typeof(PostDatabaseContext))]
	[Migration("20260328135048_UPDATE_Post_IsActive")]
	/// <inheritdoc />
	public partial class UPDATE_Post_IsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "post",
                table: "Post",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "post",
                table: "Post");
        }
    }
}
