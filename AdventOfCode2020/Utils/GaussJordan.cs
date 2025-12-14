namespace Utils;

public static class GaussJordan
{
	public static (decimal[,] Matrix, List<int> UnknownColumns) Elimination(decimal[,] matrix, decimal epsilon = 0.0000000001m)
	{
		int rowCount = matrix.GetLength(0);
		int colCount = matrix.GetLength(1);
		var unknownColumns = new List<int>();
		var pivotColumn = 0;
		for (int row = 0; row < rowCount; row++)
		{
			// find pivot row & pivot column
			int pivotRow = row;
			while (pivotRow < rowCount && Math.Abs(matrix[pivotRow, pivotColumn]) < epsilon)
			{
				pivotRow++;
			}

			if (pivotRow == rowCount)
			{
				if (pivotColumn >= colCount - 1)
				{
					return (matrix, unknownColumns);
				}

				unknownColumns.Add(pivotColumn);
				pivotColumn++;
				row--;
				continue;
			}

			// move pivot row to correct position
			if (pivotRow != row)
			{
				for (int column = 0; column < colCount; column++)
				{
					var temp = matrix[row, column];
					matrix[row, column] = matrix[pivotRow, column];
					matrix[pivotRow, column] = temp;
				}
			}

			// normalize pivot row to have a leading 1
			var pivotValue = matrix[row, pivotColumn];
			for (int col = 0; col < colCount; col++)
			{
				matrix[row, col] /= pivotValue;
			}

			// set 0 in all other rows in pivot column
			for (int rowToNormalize = 0; rowToNormalize < rowCount; rowToNormalize++)
			{
				if (rowToNormalize != row)
				{
					var factor = matrix[rowToNormalize, pivotColumn];
					for (int col = 0; col < colCount; col++)
					{
						matrix[rowToNormalize, col] -= factor * matrix[row, col];
					}
				}
			}

			pivotColumn++;
		}

		// mark remaining columns as unknown
		for (int i = pivotColumn; i < colCount - 1; i++)
		{
			unknownColumns.Add(i);
		}

		return (matrix, unknownColumns);
	}
}
