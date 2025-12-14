using System.Text.RegularExpressions;
using Utils;

var passports = File.ReadAllLines("input.txt")
	.SplitByPredicate(string.IsNullOrEmpty)
	.Select(lines => string.Join(" ", lines))
	.ToList();

var requiredFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

var result1 = passports.Where(IsValid).Count();
Console.WriteLine("1: " + result1);

var result2 = passports.Where(IsValid2).Count();
Console.WriteLine("2: " + result2);

bool IsValid(string passport)
{
	return requiredFields.All(field => passport.Contains(field + ":"));
}

bool IsValid2(string passport)
{
	var values = passport.Split([" ", Environment.NewLine], StringSplitOptions.RemoveEmptyEntries)
		.Select(v => v.Split(":"))
		.ToDictionary(x => x[0], x => x[1]);

	if (!requiredFields.All(values.ContainsKey))
	{
		return false;
	}


	if (!int.TryParse(values["byr"], out var byr) || byr < 1920 || byr > 2002)
	{
		return false;
	}

	if (!int.TryParse(values["iyr"], out var iyr) || iyr < 2010 || iyr > 2020)
	{
		return false;
	}

	if (!int.TryParse(values["eyr"], out var eyr) || eyr < 2020 || eyr > 2030)
	{
		return false;
	}

	var hgt = values["hgt"];
	var hgtInCm = hgt.EndsWith("cm");
	var hgtInIn = hgt.EndsWith("in");
	var hgtClean = hgt.Replace("cm", "").Replace("in", "");
	if (!hgtInCm && !hgtInIn)
	{
		return false;
	}

	if (!int.TryParse(hgtClean, out var hgtVal)
		|| (hgtInCm && (hgtVal < 150 || hgtVal > 193))
		|| (hgtInIn && (hgtVal < 59 || hgtVal > 76)))
	{
		return false;
	}

	var hcl = values["hcl"];
	if (!hcl.StartsWith("#") || hcl.Length != 7 || !Regex.IsMatch(hcl[1..], "^[0-9a-f]{6}$"))
	{
		return false;
	}

	var ecl = values["ecl"];
	var validEcls = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
	if (!validEcls.Contains(ecl.Trim()))
	{
		return false;
	}

	var pid = values["pid"];
	if(pid.Length != 9 || !int.TryParse(pid, out var pidVal))
	{
		return false;
	}

	return true;
}