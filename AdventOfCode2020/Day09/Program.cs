
var input = File.ReadAllLines("input.txt")
	.Select(long.Parse)
	.ToList();

var invalid = FindInvalid(input, 25);
Console.WriteLine("1: " + invalid);

var sumBlock = FindSumBlock(input, invalid);
Console.WriteLine("2: " + (sumBlock.Min() + sumBlock.Max()));

long FindInvalid(List<long> input, int bufferSize)
{
	var buffer = input[0..bufferSize];
	foreach (var value in input.Skip(bufferSize))
	{
		if (!IsValid(value, buffer))
		{
			return value;
		}

		buffer.RemoveAt(0);
		buffer.Add(value);
	}

	throw new Exception();
}

bool IsValid(long value, List<long> elements)
{
	for (int i = 0; i < elements.Count; i++)
	{
		var searched = value - elements[i];
		if (elements.Contains(searched) && searched != elements[i])
		{
			return true;
		}
	}

	return false;
}

List<long> FindSumBlock(List<long> input, long desiredSum)
{
	var startIndex = 0;
	var endIndex = 0;
	var sum = input[0];

	while (sum != desiredSum)
	{
		if (sum < desiredSum)
		{
			endIndex++;
			sum += input[endIndex];
		}

		if (sum > desiredSum)
		{
			sum -= input[startIndex];
			startIndex++;
		}
	}

	return input[startIndex..(endIndex + 1)];
}