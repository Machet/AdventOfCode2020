
var seatIds = File.ReadAllLines("input.txt")
	.Select(CalculateSeatId)
	.Order()
	.ToList();

Console.WriteLine("1: " + seatIds.Max());

var missingSeat = Enumerable.Range(seatIds.Min(), seatIds.Max() - seatIds.Min()).Except(seatIds);

Console.WriteLine("2: " + missingSeat.First());

int CalculateSeatId(string pass)
	=> GetRowId(pass) * 8 + GetColumnId(pass);

int GetRowId(string pass)
	=> Eliminate(0, 127, pass[0..7], 'B', 'F');

int GetColumnId(string pass)
	=> Eliminate(0, 7, pass[7..^0], 'R', 'L');

int Eliminate(int start, int end, string text, char up, char down)
{
	var middle = (end + start) / 2;
	var toConsider = text.FirstOrDefault();

	if (toConsider == down)
	{
		return Eliminate(start, middle, text[1..^0], up, down);
	}

	if (toConsider == up)
	{
		return Eliminate(middle + 1, end, text[1..^0], up, down);
	}

	return start;
}