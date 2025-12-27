
namespace Utils;

public record Point(int X, int Y)
{
	public bool IsWithin<T>(T[,] array)
	{
		return X >= 0 && X < array.GetLength(0) && Y >= 0 && Y < array.GetLength(1);
	}

	public Point GetNorthOf(int dist = 1)
	{
		return new Point(X - dist, Y);
	}

	public Point GetNorthWestOf(int dist = 1)
	{
		return new Point(X - dist, Y - dist);
	}

	public Point GetNorthEastOf(int dist = 1)
	{
		return new Point(X - dist, Y + dist);
	}

	public Point GetSouthOf(int dist = 1)
	{
		return new Point(X + dist, Y);
	}

	public Point GetSouthWestOf(int dist = 1)
	{
		return new Point(X + dist, Y - dist);
	}

	public Point GetSouthEastOf(int dist = 1)
	{
		return new Point(X + dist, Y + dist);
	}

	public Point GetWestOf(int dist = 1)
	{
		return new Point(X, Y - dist);
	}

	public Point GetEastOf(int dist = 1)
	{
		return new Point(X, Y + dist);
	}

	public IEnumerable<Point> GetNeighbours()
	{
		yield return GetNorthOf();
		yield return GetEastOf();
		yield return GetSouthOf();
		yield return GetWestOf();
	}

	public HashSet<Point> GetArea(int reach)
	{
		var visited = new HashSet<Point>();
		var toInspect = new HashSet<Point>() { this };

		for (int i = 0; i < reach; i++)
		{
			toInspect = toInspect.SelectMany(p => p.GetNeighbours()).Where(n => !visited.Contains(n)).ToHashSet();
			visited.AddRange(toInspect);
		}

		return visited;
	}

	public Point GetInDirection(MapDirection direction, int dist = 1)
	{
		return direction switch
		{
			_ when direction == MapDirection.North => GetNorthOf(dist),
			_ when direction == MapDirection.East => GetEastOf(dist),
			_ when direction == MapDirection.South => GetSouthOf(dist),
			_ when direction == MapDirection.West => GetWestOf(dist),
			_ when direction == MapDirection.NorthEast => GetNorthEastOf(dist),
			_ when direction == MapDirection.SouthEast => GetSouthEastOf(dist),
			_ when direction == MapDirection.SouthWest => GetSouthWestOf(dist),
			_ when direction == MapDirection.NorthWest => GetNorthWestOf(dist),
			_ => throw new NotImplementedException()
		};
	}

	public int ManhattanDistance(Point end)
	{
		return Math.Abs(X - end.X) + Math.Abs(Y - end.Y);
	}

	public static Vector operator *(Point a, int value) => new Vector(a.X * value, a.Y * value);
	public static Vector operator +(Point a, Point b) => new Vector(a.X + b.X, a.Y + b.Y);
	public static Vector operator -(Point a, Point b) => new Vector(a.X - b.X, a.Y - b.Y);
	public static Point operator +(Point point, Vector vec) => new Point(point.X + vec.X, point.Y + vec.Y);
	public static Point operator -(Point point, Vector vec) => new Point(point.X - vec.X, point.Y - vec.Y);

}