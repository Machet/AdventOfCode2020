using Utils;

var instructions = File.ReadAllLines("input.txt")
	.Select(ParseInstruciton)
	.ToList();

Point position = MoveShip1(instructions);
Console.WriteLine("1: " + position.ManhattanDistance(new Point(0, 0)));

Point position2 = MoveShip2(instructions);
Console.WriteLine("2: " + position2.ManhattanDistance(new Point(0, 0)));

static Point MoveShip1(List<Instruction> instructions)
{
	var facingDirection = MapDirection.East;
	var position = new Point(0, 0);

	foreach (var instruction in instructions)
	{
		(facingDirection, position) = instruction.Type switch
		{
			'N' => (facingDirection, position.GetNorthOf(instruction.Value)),
			'S' => (facingDirection, position.GetSouthOf(instruction.Value)),
			'E' => (facingDirection, position.GetEastOf(instruction.Value)),
			'W' => (facingDirection, position.GetWestOf(instruction.Value)),
			'L' => (facingDirection.TurnL(instruction.Value), position),
			'R' => (facingDirection.TurnR(instruction.Value), position),
			'F' => (facingDirection, position.GetInDirection(facingDirection, instruction.Value)),
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	return position;
}

static Point MoveShip2(List<Instruction> instructions)
{
	var position = new Point(0, 0);
	var wayPoint = position.GetEastOf(10).GetNorthOf(1);

	foreach (var instruction in instructions)
	{
		if (instruction.Type == 'F')
		{
			position = position + wayPoint * instruction.Value;
			continue;
		}

		wayPoint = instruction.Type switch
		{
			'N' => wayPoint.GetNorthOf(instruction.Value),
			'S' => wayPoint.GetSouthOf(instruction.Value),
			'E' => wayPoint.GetEastOf(instruction.Value),
			'W' => wayPoint.GetWestOf(instruction.Value),
			'L' => TurnL(wayPoint, instruction.Value),
			'R' => TurnR(wayPoint, instruction.Value),
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	return position;
}

static Point TurnR(Point point, int degrees)
{
	if (degrees % 90 != 0 || degrees < 0) throw new ArgumentException();

	if (degrees == 0)
	{
		return point;
	}

	return TurnR(new Point(point.Y, -point.X), degrees - 90);
}

static Point TurnL(Point point, int degrees)
{
	if (degrees % 90 != 0 || degrees < 0) throw new ArgumentException();

	if (degrees == 0)
	{
		return point;
	}

	return TurnL(new Point(-point.Y, point.X), degrees - 90);
}


Instruction ParseInstruciton(string text)
{
	return new Instruction(text[0], int.Parse(text[1..^0]));
}

record Instruction(char Type, int Value);