using SocialLink.SharedKernel.Domain;

namespace SocialLink.SharedKernel.Extensions;
public static class ArrayExtensions
{
	public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source) => source?.Where(_ => _ != null);

	public static IEnumerable<(T, int)> WithIndex<T>(this IEnumerable<T> source) => source?.Select((item, index) => (item, index));

	public static List<TKey> SelectIds<T, TKey>(this IEnumerable<IEntity<TKey>> source)
		where T : class
		where TKey : struct
		=> source?.Where(_ => _ != null).Select(_ => _.Id).Distinct().ToList();

	public static List<TKey> SelectIds<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
		where T : class
		where TKey : struct
		=> source?.Where(_ => _ != null).Select(selector).Distinct().ToList();
}
