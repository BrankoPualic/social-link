using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SocialLink.Posts.Data;

namespace SocialLink.Posts.Migrations;

[DbContext(typeof(PostDatabaseContext))]
[Migration("20260328135050_SET_Post_IsActive")]
public partial class SET_Post_IsActive : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql("UPDATE [post].[Post] SET IsActive = 1");
	}
}
