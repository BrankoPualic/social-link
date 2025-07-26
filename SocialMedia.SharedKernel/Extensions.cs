using Microsoft.EntityFrameworkCore.Migrations;
using SocialMedia.SharedKernel.Domain;

namespace SocialMedia.SharedKernel;

public static class Extensions
{
	public static void GenerateIdIfNew(this IDomainModel<Guid> model)
	{
		if (model.IsNew)
			model.Id = Guid.NewGuid();
	}

	// Data migration extension
	public static void ExecuteScript(this MigrationBuilder builder, string module, string file)
	{
		var path = Path.Combine("..", $"SocialMedia.{module}", "Data", "Scripts", file);
		var sql = File.ReadAllText(path);
		builder.Sql(sql);
	}
}