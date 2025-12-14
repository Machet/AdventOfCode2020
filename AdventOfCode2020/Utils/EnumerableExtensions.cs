namespace Utils;

public static class EnumerableExtensions
{
	public static IEnumerable<List<T>> Split<T>(this IEnumerable<T> source, int count)
	{
		return source
			.Select((x, i) => new { Index = i, Value = x })
			.GroupBy(x => x.Index / count)
			.Select(x => x.Select(v => v.Value).ToList());
	}

	public static IEnumerable<List<T>> SplitByPredicate<T>(this IEnumerable<T> source, Func<T, bool> predicate)
	{
		var currentList = new List<T>();
		foreach (var item in source)
		{
			if (predicate(item))
			{
				if (currentList.Count > 0)
				{
					yield return currentList;
					currentList = new List<T>();
				}
			}
			else
			{
				currentList.Add(item);
			}
		}
		if (currentList.Count > 0)
		{
			yield return currentList;
		}
	}

	public static IEnumerable<T> IterateInCircles<T>(this IEnumerable<T> items)
	{
		while (true)
		{
			foreach (var item in items)
			{
				yield return item;
			}
		}
	}

	public static IEnumerable<(T first, T second)> GenerateAllCombinations<T>(this IEnumerable<T> items)
	{
		var toIterate = items.ToList();
		for (int i = 0; i < toIterate.Count; i++)
		{
			for (var j = i + 1; j < toIterate.Count; j++)
			{
				yield return (toIterate[i], toIterate[j]);
			}
		}
	}

	public static IEnumerable<(T first, T second)> GenerateAllPossibleCombinations<T>(this IEnumerable<T> items)
	{
		var toIterate = items.ToList();
		for (int i = 0; i < toIterate.Count; i++)
		{
			for (var j = 0; j < toIterate.Count; j++)
			{
				yield return (toIterate[i], toIterate[j]);
			}
		}
	}

	public static TOut[,] To2DArray<TIn, TOut>(this List<List<TIn>> source)
	{
		var rowCount = source.Count;
		var colCount = source[0].Count;
		var result = new TOut[rowCount, colCount];
		for (int i = 0; i < rowCount; i++)
		{
			if (source[i].Count() != colCount)
			{
				throw new ArgumentException("All rows must have the same number of columns.");
			}

			for (int j = 0; j < colCount; j++)
			{
				result[i, j] = (TOut)(dynamic)source[i][j]!;
			}
		}

		return result;
	}

	public static void Print<T>(this IEnumerable<T> items)
	{
		foreach (var item in items)
		{
			Console.WriteLine(item);
		}
	}

	public static long Multiply(this IEnumerable<int> items)
	{
		long result = 1;
		foreach (var item in items)
		{
			result *= item;
		}

		return result;
	}
}
