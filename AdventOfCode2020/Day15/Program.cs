var numbers = File.ReadAllText("input.txt")
	.Split(",")
	.Select(int.Parse)
	.ToList();

var turnMemory = numbers.Select((n, i) => (n, i)).ToDictionary(p => p.n, p => p.i + 1);
var hasBeenSpokenMemory = new HashSet<int>(numbers.Take(numbers.Count - 1));

var turn = numbers.Count;
var lastNumberSpoken = numbers.Last();

while (turn < 30000000)
{
	turn++;
	var newSpoken = !hasBeenSpokenMemory.Add(lastNumberSpoken)
		? turn - 1 - turnMemory[lastNumberSpoken]
		: 0;

	turnMemory[lastNumberSpoken] = turn - 1;
	lastNumberSpoken = newSpoken;

	if (turn == 2020)
	{
		Console.WriteLine("1: " + lastNumberSpoken);
	}
}

Console.WriteLine("2: " + lastNumberSpoken);
