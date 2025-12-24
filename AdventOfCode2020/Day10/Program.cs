
var input = File.ReadAllLines("input.txt")
	.Select(long.Parse)
	.ToList();

input.Add(0);
input.Add(input.Max() + 3);
input.Sort();

var diffCounts = new Dictionary<long, int>();
for (int i = 1; i < input.Count; i++)
{
	var diff = input[i] - input[i - 1];
	if (!diffCounts.ContainsKey(diff))
	{
		diffCounts[diff] = 0;
	}

	diffCounts[diff] += 1;
}

Console.WriteLine("1: " + (diffCounts[3] * diffCounts[1]));

var cache = new Dictionary<long, long>();
var numberOfArrangements = GetNumberOfArrangements(input.Max(), input, cache);
Console.WriteLine("2: " + numberOfArrangements);


long GetNumberOfArrangements(long endValue, List<long> input, Dictionary<long, long> cache)
{
	if (cache.ContainsKey(endValue))
	{
		return cache[endValue];
	}

	if (!input.Contains(endValue))
	{
		return 0;
	}

	if (endValue == 0)
	{
		return 1;
	}

	var result = GetNumberOfArrangements(endValue - 3, input, cache)
		+ GetNumberOfArrangements(endValue - 2, input, cache)
		+ GetNumberOfArrangements(endValue - 1, input, cache);

	cache[endValue] = result;
	return result;
}

//0, 1, 2, 3, 4
// 1 => 1 -> 0
// 2 => 0 -> 1 -> 2
//		0 -> 2
// 3 => 0 -> 3 (1)
//		1 -> 3 (1)
//		2 -> 3 (2)
// 4 => 1 -> 4 (1)
//		2 -> 4 (2)
//		3 -> 4 (4)
// 5 => 2 -> 5 (2)
//		3 -> 5 (4)
//		4 -> 5 (7)