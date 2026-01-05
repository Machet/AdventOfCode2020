var results1 = File.ReadAllLines("input.txt")
	.Select(l => Solve1(l.GetEnumerator()))
	.ToList();

Console.WriteLine("1: " + results1.Sum());

var results2 = File.ReadAllLines("input.txt")
	.Select(l => Solve2(l.GetEnumerator()))
	.ToList();

Console.WriteLine("2: " + results2.Sum());

static long Solve1(CharEnumerator processor)
{
	var result = 0L;
	char? operation = null;

	while (processor.MoveNext())
	{
		var c = processor.Current;
		if (c == '+' || c == '*')
		{
			operation = c;
		}

		if (char.IsDigit(c))
		{
			var val = long.Parse(c.ToString());
			result = ProcessOperation(result, operation, val);
		}

		if (c == '(')
		{
			var val = Solve1(processor);
			result = ProcessOperation(result, operation, val);
		}

		if (c == ')')
		{
			return result;
		}
	}

	return result;
}

long Solve2(CharEnumerator processor, int level = 0)
{
	char? operation = null;
	long result = 0;
	var mulCache = new List<long>();

	while (processor.MoveNext())
	{
		var c = processor.Current;
		if (c == '+')
		{
			operation = c;
		}

		if (c == '*')
		{
			if (result != 0)
			{
				mulCache.Add(result);
			}

			result = 0;
		}

		if (char.IsDigit(c))
		{
			var val = long.Parse(c.ToString());
			result = ProcessOperation(result, operation, val);
		}

		if (c == '(')
		{
			var val = Solve2(processor, level + 1);
			result = ProcessOperation(result, operation, val);
		}

		if (c == ')')
		{
			return ProcessSecondLevelMul(result, mulCache);
		}
	}

	return ProcessSecondLevelMul(result, mulCache);
}

static long ProcessOperation(long val1, char? operation, long val2)
{
	if (operation == null)
	{
		if (val1 != 0) throw new InvalidOperationException();
		return val2;
	}
	else
	{
		return operation switch
		{
			'+' => val1 + val2,
			'*' => val1 * val2,
			_ => throw new InvalidOperationException()
		};
	}
}

long ProcessSecondLevelMul(long currentResult, List<long> mulCache)
{
	var mulResult = mulCache.Aggregate(1L, (mul, val) => mul * val);
	return currentResult != 0 ? currentResult * mulResult : mulResult;
}