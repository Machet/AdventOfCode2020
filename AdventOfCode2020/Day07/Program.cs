
using System.Data;
using System.Text.RegularExpressions;

var rules = File.ReadAllLines("input.txt")
	.Select(ParseRules)
	.ToList();

var cache1 = new Dictionary<string, bool>();
var containingShiny = rules.Where(r => CanContainBag(r.Bag, "shiny gold", rules, cache1)).ToList();
Console.WriteLine("1: " + containingShiny.Count());

var cache2 = new Dictionary<string, long>();
var inShinyBagCount = GetBagCount("shiny gold", rules, cache2);
Console.WriteLine("2: " + inShinyBagCount);

bool CanContainBag(string bag, string searchedBag, List<Rule> rules, Dictionary<string, bool> cache)
{
	if (cache.ContainsKey(bag))
	{
		return cache[bag];
	}

	var content = rules.First(r => r.Bag == bag).Content;
	if (content.Any(c => c.Bag == searchedBag))
	{
		cache[bag] = true;
		return cache[bag];
	}

	cache[bag] = content.Any(c => CanContainBag(c.Bag, searchedBag, rules, cache));
	return cache[bag];
}

Rule ParseRules(string line)
{
	string bagPattern = @"^(?<container>[\w\s]+) bags contain (?<contents>.+)\.$";
	string contentsPattern = @"(?<count>\d+) (?<color>[\w\s]+?) bags?";

	var match = Regex.Match(line, bagPattern);
	string container = match.Groups["container"].Value.Trim();
	string contents = match.Groups["contents"].Value;

	var contentMatches = Regex.Matches(contents, contentsPattern);
	var content = contentMatches
		.Select(match => new BagCount(
			match.Groups["color"].Value.Trim(),
			int.Parse(match.Groups["count"].Value)))
		.ToList();

	return new Rule(container, content);
}

long GetBagCount(string searchedBag, List<Rule> rules, Dictionary<string, long> cache)
{
	if (cache.ContainsKey(searchedBag))
	{
		return cache[searchedBag];
	}

	var rule = rules.First(r => r.Bag == searchedBag);

	var result = rule.Content.Count == 0
		? 0
		: rule.Content.Select(c => GetBagCount(c.Bag, rules, cache) * c.Count + c.Count).Sum();

	cache[searchedBag] = result;
	return result;
}

record Rule(string Bag, List<BagCount> Content);
record BagCount(string Bag, int Count);