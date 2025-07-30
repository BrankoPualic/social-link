using HeyRed.Mime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MongoDB.Driver;
using SocialMedia.SharedKernel.Domain;
using System.Linq.Expressions;

namespace SocialMedia.SharedKernel;

public static class Extensions
{
	public static void GenerateIdIfNew(this IDomainModel<Guid> model)
	{
		if (model.IsNew)
			model.Id = Guid.NewGuid();
	}

	public static string ToMimeType(this string value) => MimeTypesMap.GetMimeType(value);

	public static string GetAzureFileRelativePath(this string file, string containerName) => file[(file.IndexOf(containerName) + containerName.Length + 1)..];

	// Data migration extensions

	public static void ExecuteScript(this MigrationBuilder builder, string module, string file)
	{
		var path = Path.Combine("..", $"SocialMedia.{module}", "Data", "Scripts", file);
		var sql = File.ReadAllText(path);
		builder.Sql(sql);
	}

	// Data access extensions

	// TODO: Investigate PagedResult<T> from Ardalis.Result library
	public static async Task<PagedResponse<TResponse>> EFSearchAsync<TResponse, TProperty, TModel>(
		this DbSet<TModel> dbSet,
		PagedSearch search,
		Expression<Func<TModel, TProperty>> defaultOrder,
		bool desc,
		Expression<Func<TModel, TResponse>> projection,
		List<Expression<Func<TModel, bool>>> predicates,
		CancellationToken ct
	)
		where TModel : class
	{
		var query = dbSet.AsQueryable();
		query = predicates.Aggregate(query, (current, expression) => current.Where(expression));

		var total = await query.CountAsync(ct);

		query = desc
			? query.OrderByDescending(defaultOrder)
			: query.OrderBy(defaultOrder);

		query = query
			.Skip((search.Page - 1) * search.PageSize)
			.Take(search.PageSize);

		return new()
		{
			TotalCount = total,
			CurrentPage = search.Page,
			PageSize = search.PageSize,
			Items = await query.Select(projection).ToListAsync(ct)
		};
	}

	public static async Task<PagedResponse<TResponse>> MongoSearchAsync<TModel, TResponse>(
		this IMongoCollection<TModel> collection,
		PagedSearch search,
		Expression<Func<TModel, object>> defaultOrder,
		bool desc,
		Expression<Func<TModel, TResponse>> projection,
		FilterDefinition<TModel> filter,
		CancellationToken ct
	)
		where TModel : class
	{
		var total = await collection.CountDocumentsAsync(filter, cancellationToken: ct);

		var sort = desc
			? Builders<TModel>.Sort.Descending(defaultOrder)
			: Builders<TModel>.Sort.Ascending(defaultOrder);

		var items = await collection
			.Find(filter)
			.Sort(sort)
			.Skip((search.Page - 1) * search.PageSize)
			.Limit(search.PageSize)
			.Project(projection)
			.ToListAsync(ct);

		return new PagedResponse<TResponse>
		{
			TotalCount = total,
			CurrentPage = search.Page,
			PageSize = search.PageSize,
			Items = items
		};
	}

	// Array

	public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source) => source?.Where(_ => _ != null);

	public static IEnumerable<(T, int)> WithIndex<T>(this IEnumerable<T> source) => source?.Select((item, index) => (item, index));
}