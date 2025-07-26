namespace SocialMedia.SharedKernel;

public record PagedSearch
{
	private int _pageSize = 10;
	private int _page = 1;
	public int PageSize
	{
		get => _pageSize;
		set
		{
			if (value > 0)
				_pageSize = value;
		}
	}

	public int Page
	{
		get => _page;
		set
		{
			if (value > 0)
				_page = value;
		}
	}
}