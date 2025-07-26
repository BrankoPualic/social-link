namespace SocialMedia.SharedKernel;

public record PagedResponse<T>
{
	public long TotalCount { get; set; }

	public int CurrentPage { get; set; }

	public int PageSize { get; set; }

	public int PageCount => (int)Math.Ceiling((float)TotalCount / PageSize);

	public IEnumerable<T> Items { get; set; } = [];
}