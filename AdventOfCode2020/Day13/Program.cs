var lines = File.ReadAllLines("input.txt")
	.ToList();

var searchedTime = ulong.Parse(lines[0]);
var departureTimes = lines[1].Split(",").Where(x => x != "x").Select(ulong.Parse).ToList();

var waitTimes = departureTimes
	.Select(dt => GetWaitTime(dt, searchedTime))
	.ToList();

var best = waitTimes.OrderBy(dt => dt.waitTime).First();
Console.WriteLine("1: " + (best.bus * best.waitTime));

var busLines = lines[1].Split(",")
	.Select((value, i) => (value, i))
	.Where(x => x.value != "x")
	.Select(x => (value: ulong.Parse(x.value), i: (ulong)x.i))
	.ToList();

var time = busLines.First().value;
var multiply = time;

foreach(var line in busLines.Skip(1).OrderByDescending(l => l.value))
{
	var rest = time % line.value;
	var expectedRest = line.value - (line.i % line.value);
	while(rest != expectedRest)
	{
		time += multiply;
		rest = time % line.value;
	}

	multiply = multiply * line.value;
}

Console.WriteLine("2: " + time);

(ulong bus, ulong waitTime) GetWaitTime(ulong departure, ulong searchedTime)
{
	var departTime = (searchedTime / departure) + 1;
	var departAt = departure * departTime;
	return (departure, departAt - searchedTime);
}