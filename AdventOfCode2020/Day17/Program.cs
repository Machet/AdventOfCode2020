using Utils;

var initalState = File.ReadAllLines("input.txt")
	.ToCharArray()
	.SelectItems()
	.Where(i => i.Item == '#')
	.Select(i => i.Point)
	.ToList();

var state3D = initalState.Select(p => (Point)new Point3D(p.X, p.Y, 0)).ToHashSet();
var state4D = initalState.Select(p => (Point)new Point4D(p.X, p.Y, 0, 0)).ToHashSet();

for (int i = 0; i < 6; i++)
{
	state3D = GetNewState(state3D);
	state4D = GetNewState(state4D);
}

Console.WriteLine("1: " + state3D.Count);
Console.WriteLine("2: " + state4D.Count);

HashSet<Point> GetNewState(HashSet<Point> state)
{
	var toConsider = state.SelectMany(s => s.GetNeighbours(true)).ToHashSet();
	return toConsider.Where(p => p.ShouldBeActive(state)).ToHashSet();
}

abstract record Point
{
	public abstract IEnumerable<Point> GetNeighbours(bool includeCenter = false);

	public bool ShouldBeActive(HashSet<Point> activeCubes)
	{
		var isActive = activeCubes.Contains(this);
		var activeNeighbourCount = GetNeighbours().Count(activeCubes.Contains);
		return isActive
			? activeNeighbourCount == 2 || activeNeighbourCount == 3
			: activeNeighbourCount == 3;
	}
}

record Point3D(int X, int Y, int Z) : Point
{
	public override IEnumerable<Point> GetNeighbours(bool includeCenter = false)
	{
		for (var i = -1; i <= 1; i++)
			for (var j = -1; j <= 1; j++)
				for (var k = -1; k <= 1; k++)
				{
					var isBoundary = i != 0 || j != 0 || k != 0;
					if (isBoundary || includeCenter)
					{
						yield return new Point3D(X + i, Y + j, Z + k);
					}
				}
	}
}

record Point4D(int X, int Y, int Z, int W) : Point
{
	public override IEnumerable<Point> GetNeighbours(bool includeCenter = false)
	{
		for (var i = -1; i <= 1; i++)
			for (var j = -1; j <= 1; j++)
				for (var k = -1; k <= 1; k++)
					for (var l = -1; l <= 1; l++)
					{
						var isBoundary = i != 0 || j != 0 || k != 0 || l != 0;
						if (isBoundary || includeCenter)
						{
							yield return new Point4D(X + i, Y + j, Z + k, W + l);
						}
					}
	}
}