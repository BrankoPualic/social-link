namespace SocialLink.SharedKernel;

public class LookupDto
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public static LookupDto EnumProjection<T>(T value) where T : Enum => new()
	{
		Id = Convert.ToInt32(value),
		Name = value.ToString(),
		Description = value.GetDescription()
	};
}