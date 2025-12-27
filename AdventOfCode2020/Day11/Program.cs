using Utils;

var seats = File.ReadAllLines("input.txt")
	.ToCharArray();

var neighbourSeats = GetNeighbourSeatsMap(seats);
int occupiedSeats1 = GetOccupiedSeats(seats, neighbourSeats, 4);
Console.WriteLine("1: " + occupiedSeats1);

var visibleSeats = GetVisibleSeatsMap(seats);
int occupiedSeats2 = GetOccupiedSeats(seats, visibleSeats, 5);
Console.WriteLine("2: " + occupiedSeats2);

int GetOccupiedSeats(char[,] seats, Dictionary<Point, List<Point>> neighbourSeats, int leaveWhenNeighbourCount)
{
	var nextSeats = seats;

	do
	{
		seats = nextSeats;
		nextSeats = seats.Select((pos, x) => GetSeatState(pos, neighbourSeats[pos], seats, leaveWhenNeighbourCount));
	}
	while (!nextSeats.HasEqualValues(seats));

	var occupiedSeats = seats.SelectItems()
		.Where(i => i.Item == '#')
		.Count();

	return occupiedSeats;
}

char GetSeatState(Point pos, List<Point> seatsToConsider, char[,] seats, int leaveWhenNeighbourCount)
{
	var value = seats.GetItem(pos).Item;
	var seatsState = seats.ItemsOnPosition(seatsToConsider);

	if (value == 'L' && seatsState.Where(n => n.Item == '#').Count() == 0)
	{
		return '#';
	}

	if (value == '#' && seatsState.Where(n => n.Item == '#').Count() >= leaveWhenNeighbourCount)
	{
		return 'L';
	}

	return value;
}

Dictionary<Point, List<Point>> GetNeighbourSeatsMap(char[,] seats)
{
	return seats.SelectItems()
		.ToDictionary(i => i.Point, i => seats.GetNeighbours(i.Point, true).ToList());
}


Dictionary<Point, List<Point>> GetVisibleSeatsMap(char[,] seats)
{
	return seats.SelectItems()
		.ToDictionary(i => i.Point, i => GetVisibleSeats(i.Point, seats).ToList());
}

IEnumerable<Point> GetVisibleSeats(Point point, char[,] seats)
{
	List<Point?> visibleSeats = [
		GetSeatInDir("N", point, seats),
		GetSeatInDir("S", point, seats),
		GetSeatInDir("W", point, seats),
		GetSeatInDir("E", point, seats),
		GetSeatInDir("SE", point, seats),
		GetSeatInDir("NE", point, seats),
		GetSeatInDir("SW", point, seats),
		GetSeatInDir("NW", point, seats),
	];

	return visibleSeats
		.Where(p => p != null)
		.Select(p => p!);
}

Point? GetSeatInDir(string dir, Point startingPos, char[,] seats)
{
	var next = seats.GetInDirection(startingPos, dir);
	while (next?.Item == '.')
	{
		next = seats.GetInDirection(next.Point, dir);
	}

	return next?.Point;
}