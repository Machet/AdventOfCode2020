var passwords = File.ReadAllLines("input.txt")
	.Select(Parse)
	.ToList();

var result1 = passwords.Where(p => p.IsValid1()).Count();
var result2 = passwords.Where(p => p.IsValid2()).Count();

Console.WriteLine("1: " +  result1);
Console.WriteLine("2: " +  result2);

PasswordLine Parse(string x)
{
	var parts = x.Split(" ");
	var rangeParts = parts[0].Split("-");
	return new PasswordLine(int.Parse(rangeParts[0]), int.Parse(rangeParts[1]), parts[1].First(), parts[2]);
}

public record PasswordLine(int policyLow, int policyHigh, char letter, string password)
{
	public bool IsValid1()
	{
		var count = password.Where(c => c == letter).Count();
		return count >= policyLow && count <= policyHigh;
	}

	public bool IsValid2()
	{
		var n1 = password[policyLow - 1];
		var n2 = password[policyHigh - 1];
		return  (n1 == letter ||  n2 == letter) && n1 != n2;
	}
}