using Utils;

var operations = File.ReadAllLines("input.txt")
	.Select(Parse)
	.ToList();

var memory = new Dictionary<string, ulong>();
foreach (var operation in operations)
{
	operation.Run(memory);
}

var sum = memory.Where(m => !m.Key.Contains("mask")).Select(m => (decimal)m.Value).Sum();
Console.WriteLine("1: " + sum);

var maskedSets = GetSetMemoryMasked(operations).ToList();
maskedSets.Reverse();

var processed = new List<SetMemory>();
foreach (var set in maskedSets)
{
	var resolved = set.ResolveConflict(processed);
	processed.AddRange(resolved);
}

var result2 = processed.Select(x => (decimal)x.GetSumOfSetValues()).Sum();
Console.WriteLine("2: " + result2);

Operation Parse(string text)
{
	var parts = text.Split("=");
	if (parts[0].StartsWith("mask"))
	{
		return new SetMask(parts[1]);
	}

	var address = parts[0].Replace("mem[", "").Replace("]", "");
	return new SetMemory(address, ulong.Parse(parts[1]));
}

static IEnumerable<SetMemory> GetSetMemoryMasked(List<Operation> operations)
{
	SetMask? currentMask = null;
	foreach (var operation in operations)
	{
		if (operation is SetMask setMask)
		{
			currentMask = setMask;
			continue;
		}

		yield return ((SetMemory)operation).WithMask(currentMask ?? throw new ArgumentException());
	}
}

abstract record Operation()
{
	public abstract void Run(Dictionary<string, ulong> memory);
}

record SetMask(string Mask) : Operation
{
	public override void Run(Dictionary<string, ulong> memory)
	{
		memory["0mask"] = GetZeroMask();
		memory["1mask"] = GetOneMask();
	}

	private ulong GetOneMask()
	{
		var mask = 0ul;
		for (int i = 0; i < Mask.Length; i++)
		{
			if (Mask[^(i + 1)] == '1')
			{
				mask |= 1ul << i;
			}
		}

		return mask;
	}

	private ulong GetZeroMask()
	{
		var mask = ulong.MaxValue;
		for (int i = 0; i < Mask.Length; i++)
		{
			if (Mask[^(i + 1)] == '0')
			{
				mask &= ~(1ul << i);
			}
		}

		return mask;
	}
}

record SetMemory(string Address, ulong Value) : Operation
{
	public override void Run(Dictionary<string, ulong> memory)
	{
		var value = Value;
		value &= memory["0mask"];
		value |= memory["1mask"];
		memory[Address] = value;
	}

	internal SetMemory WithMask(SetMask mask)
	{
		var binaryAddress = Convert.ToString(int.Parse(Address), 2);
		var maskedAddres = mask.Mask.ToCharArray();

		for (int i = 0; i < mask.Mask.Length; i++)
		{
			if (maskedAddres[^(i + 1)] == '0')
			{
				maskedAddres[^(i + 1)] = i < binaryAddress.Length
					? binaryAddress[^(i + 1)]
					: '0';
			}
		}

		return new SetMemory(new string(maskedAddres), Value);
	}

	public bool HasConflict(SetMemory another)
	{
		if (another.Address.Length != Address.Length)
		{
			throw new InvalidOperationException();
		}

		for (int i = 0; i < Address.Length; i++)
		{
			var match = Address[i] == another.Address[i]
				|| Address[i] == 'X'
				|| another.Address[i] == 'X';

			if (!match)
			{
				return false;
			}
		}

		return true;
	}

	internal List<SetMemory> ResolveConflict(List<SetMemory> processed)
	{
		var conflicts = processed.Where(p => p.HasConflict(this)).ToList();
		if (!conflicts.Any())
		{
			return [this];
		}

		var conflict = conflicts.First().Address;

		var posToReplace = GetPosOfFluidRangeDiff(conflict);
		if (posToReplace == null)
		{
			return [];
		}

		var first = new SetMemory(Address.ReplaceAt(posToReplace.Value, '0'), Value);
		var second = new SetMemory(Address.ReplaceAt(posToReplace.Value, '1'), Value);
		var result = first.ResolveConflict(processed);
		result.AddRange(second.ResolveConflict(processed));
		return result;
	}

	private int? GetPosOfFluidRangeDiff(string another)
	{
		for (int i = 0; i < Address.Length; i++)
		{
			if (another[i] == 'X' || another[i] == Address[i])
			{
				continue;
			}
			else if (Address[i] == 'X')
			{
				return i;
			}
		}

		return null;
	}

	internal ulong GetSumOfSetValues()
	{
		var fluidBits = Address.Where(c => c == 'X').Count();
		var addressRange = (ulong)Math.Pow(2, fluidBits);
		return Value * addressRange;
	}
}
