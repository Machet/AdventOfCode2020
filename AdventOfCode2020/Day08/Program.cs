
using System.Collections.Immutable;

var instructions = File.ReadAllLines("input.txt")
	.Select(Parse)
	.ToImmutableList();

(var acc, var endless) = RunProgram(instructions);
Console.WriteLine("1: " + acc);

for (int i = 0; i < instructions.Count; i++)
{
	var instruction = instructions[i];
	if (instruction.Type == "acc") continue;

	var corrected = instructions.Replace(instruction, CorrectInstruction(instruction));
	var result = RunProgram(corrected);
	if (!result.endless)
	{
		Console.WriteLine("2: " + result.acc);
		return;
	}
}

Instruction Parse(string line)
{
	var param = line.Split(" ");
	return new Instruction(param[0], int.Parse(param[1]));
}

static (int acc, bool endless) RunProgram(IList<Instruction> instructions)
{
	var instructionIndex = 0;
	var acc = 0;
	var visited = new HashSet<int>();

	while (true)
	{
		if (!visited.Add(instructionIndex))
		{
			return (acc, true);
		}

		if (instructionIndex >= instructions.Count)
		{
			return (acc, false);
		}

		var instruction = instructions[instructionIndex];
		acc += instruction.Type == "acc" ? instruction.Value : 0;
		instructionIndex += instruction.Type == "jmp" ? instruction.Value : 1;
	}
}

Instruction CorrectInstruction(Instruction instruction)
	=> new Instruction(instruction.Type == "nop" ? "jmp" : "nop", instruction.Value);

record Instruction(string Type, int Value);