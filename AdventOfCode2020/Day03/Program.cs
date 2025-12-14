using Utils;

var input = File.ReadAllLines("input.txt")
	.ToCharArray();

int treeCount1 = CountTreesOnSlope(input, 1, 3);
Console.WriteLine("1: " + treeCount1);

long treeCount2 = CountTreesOnSlope(input, 1, 1)
	* CountTreesOnSlope(input, 1, 3)
	* CountTreesOnSlope(input, 1, 5)
	* CountTreesOnSlope(input, 1, 7)
	* CountTreesOnSlope(input, 2, 1);

Console.WriteLine("2: " + treeCount2);

static int CountTreesOnSlope(char[,] input, int down, int right)
{
	var vPos = 0;
	var treeCount = 0;
	for (int hPos = 0; hPos < input.GetLength(0); hPos += down)
	{
		treeCount = input[hPos, vPos % input.GetLength(1)] == '#' ? treeCount + 1 : treeCount;
		vPos += right;
	}

	return treeCount;
}