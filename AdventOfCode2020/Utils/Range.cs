namespace Utils;

public record LongRange(long start, long end)
{
	public long Length => end - start + 1;

	public bool Contains(long number) => start <= number && number <= end;
	public bool Contains(LongRange other) => start <= other.start && other.end <= end;
	public bool Overlaps(LongRange other) => start <= other.end && other.start <= end;
	public bool IsAdjacent(LongRange other) => end + 1 == other.start || other.end + 1 == start;

	public IEnumerable<LongRange> Merge(LongRange other)
	{
		if (!Overlaps(other) && !IsAdjacent(other))
		{
			yield return this;
			yield return other;
			yield break;
		}

		var newStart = Math.Min(start, other.start);
		var newEnd = Math.Max(end, other.end);
		yield return new LongRange(newStart, newEnd);
	}
}
