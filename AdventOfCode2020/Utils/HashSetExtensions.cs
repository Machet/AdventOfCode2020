namespace Utils;

public static class HashSetExtensions
{
	public static HashSet<T> AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
	{
		foreach (var item in items)
		{
			set.Add(item);
		}

		return set;
	}

	public static HashSet<T> RemoveRange<T>(this HashSet<T> set, IEnumerable<T> items)
	{
		foreach (var item in items)
		{
			set.Remove(item);
		}

		return set;
	}
}
