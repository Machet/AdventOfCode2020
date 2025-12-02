var numbers = File.ReadAllLines("input.txt")
	.Select(long.Parse)
	.ToHashSet();

var list = numbers.ToList();

foreach (var number in numbers)
{
	var match = FindMatch(numbers, number, 2020);
	if (match.HasValue)
	{
		Console.WriteLine("1: " + number * match);
		break;
	}
}

for (int i = 0; i < numbers.Count; i++)
{
	for (int j = 0; j < numbers.Count; j++)
	{
		var match = FindMatch(numbers, list[i] + list[j], 2020);

		if (match.HasValue)
		{
			Console.WriteLine("2: " + list[i] * list[j] * match);
			return;
		}
	}
}

static long? FindMatch(HashSet<long> numbers, long number, long sum)
{
	var match = sum - number;
	return numbers.Contains(match) ? match : null;
}