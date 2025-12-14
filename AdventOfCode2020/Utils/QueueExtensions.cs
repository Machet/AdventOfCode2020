namespace Utils;
public static class QueueExtensions
{
	public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
	{
		foreach (var item in items)
		{
			queue.Enqueue(item);
		}
	}

	public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> items)
	{
		foreach (var item in items)
		{
			stack.Push(item);
		}
	}
}
