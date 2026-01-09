using Utils;

var inputParts = File.ReadLines("input.txt")
	.SplitByPredicate(string.IsNullOrEmpty)
	.ToList();

var ruleDefinitions = inputParts[0];
var textToCheck = inputParts[1];

var rules = ParseRules(ruleDefinitions);
var zeroRule = new LengthRule(rules["0"]);

var results = textToCheck.Select(t => (t, zeroRule.Match(t).match)).ToList();
Console.WriteLine("1: " + results.Count(x => x.match));

rules["8"] = new OrRule(rules["42"], new AndRule(rules["42"], new InvokeRule("8", rules)));
rules["11"] = new OrRule(
	new AndRule(rules["42"], rules["31"]),
	new AndRule(new AndRule(rules["42"], new InvokeRule("11", rules)), rules["31"]	)
);

var results2 = textToCheck.Select(t => (t, zeroRule.Match(t).match)).ToList();
Console.WriteLine("2: " + results2.Count(x => x.match));

Dictionary<string, Rule> ParseRules(List<string> ruleDefinitions)
{
	var rules = new Dictionary<string, Rule>();
	foreach (var def in ruleDefinitions)
	{
		var parts = def.Split(":");
		rules[parts[0]] = ParseRule(parts[1], rules);
	}

	return rules;
}

Rule ParseRule(string rule, Dictionary<string, Rule> rules)
{
	if (rule.Contains("|"))
	{
		var partsOr = rule.Split("|");
		return new OrRule(ParseRule(partsOr[0], rules), ParseRule(partsOr[1], rules));
	}

	if (rule.Contains("\""))
	{
		return new MatchRule(rule[2]);
	}

	var partsAnd = rule.Trim().Split(" ");

	if (partsAnd.Length == 1)
	{
		return new InvokeRule(partsAnd[0], rules);
	}

	return new AndRule(ParseRule(partsAnd[0], rules), ParseRule(partsAnd[1], rules));
}

abstract record Rule
{
	public (bool match, List<int> pos) Match(string text) => Match(text, [0]);
	public abstract (bool match, List<int> pos) Match(string text, List<int> pos);
}

record InvokeRule(string ruleName, Dictionary<string, Rule> rules) : Rule
{
	public override (bool match, List<int> pos) Match(string text, List<int> pos)
		=> rules[ruleName].Match(text, pos);
}

record MatchRule(char chr) : Rule
{
	public override (bool, List<int>) Match(string text, List<int> pos)
	{
		var matching = pos
			.Where(p => p < text.Length && text[p] == chr)
			.Select(p => p + 1)
			.ToList();

		return (matching.Any(), matching);
	}
}

record AndRule(Rule first, Rule second) : Rule
{
	public override (bool, List<int>) Match(string text, List<int> pos)
	{
		var matching = pos.Select(p =>
		{
			var r1 = first.Match(text, [p]);
			return !r1.match ? (match: false, pos: new List<int>()) : second.Match(text, r1.pos);
		}).Where(r => r.match).ToList();

		if (!matching.Any())
		{
			return (false, new List<int>());
		}

		return (true, matching.SelectMany(r => r.pos).ToList());
	}
}

record OrRule(Rule first, Rule second) : Rule
{
	public override (bool match, List<int> pos) Match(string text, List<int> pos)
	{
		var result1 = first.Match(text, pos);
		var result2 = second.Match(text, pos);
		if (result1.match && result2.match)
		{
			return (true, result1.pos.Union(result2.pos).ToList());
		}

		return result1.match ? result1 : result2;
	}
}

record LengthRule(Rule inner) : Rule
{
	public override (bool match, List<int> pos) Match(string text, List<int> pos)
	{
		var result = inner.Match(text, pos);
		return (result.match && result.pos.Any(p => p == text.Length), result.pos);
	}
}