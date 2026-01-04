using Utils;

var parts = File.ReadAllLines("input.txt")
	.SplitByPredicate(string.IsNullOrEmpty)
	.ToList();

var rules = parts[0].Select(ParseRule).ToList();
var yourTicket = ParseTicket(parts[1][1]);
var nearbyTickets = parts[2].Skip(1).Select(ParseTicket).ToList();

var invalidAttributes = nearbyTickets.Select(t => t.GetInvalidAttributes(rules)).ToList();
Console.WriteLine("1: " + invalidAttributes.Sum());

var validTickets = nearbyTickets.Where(t => t.GetInvalidAttributes(rules) == 0).ToList();
validTickets.Add(yourTicket);

var attributesByPosition = validTickets.SelectMany(t => t.GetAttributeIndex())
	.GroupBy(a => a.position)
	.Select(g => (position: g.Key, attributes: g.Select(x => x.value).ToList()))
	.ToList();

var matchingRules = attributesByPosition
	.OrderBy(x => x.position)
	.Select(ap => (ap.position, rules: rules.Where(r => ap.attributes.All(a => r.IsValid(a))).ToList()))
	.ToList();

var positionsByRules = matchingRules.SelectMany(mr => mr.rules.Select(r => (rule: r, mr.position)))
	.GroupBy(g => g.rule)
	.Select(g => (g.Key.Name, pos: g.Select(gg => gg.position).ToList()))
	.OrderBy(g => g.pos.Count())
	.ToList();

var rulePoisitions = new List<(string name, int position)>();

while (positionsByRules.Any(r => r.pos.Count == 1))
{
	var validRule = positionsByRules.FirstOrDefault(r => r.pos.Count == 1);
	var position = validRule.pos.Single();
	rulePoisitions.Add((validRule.Name, position));

	foreach (var posByRule in positionsByRules)
	{
		posByRule.pos.Remove(position);
	}
}

var departureCombination = rulePoisitions.Where(r => r.name.Contains("departure"))
	.Select(r => yourTicket.Attributes[r.position])
	.Aggregate(1l, (mul, val) => mul * val);

Console.WriteLine("2: " + departureCombination);

Rule ParseRule(string line)
{
	var parts = line.Split(":");
	var rangeParts = parts[1].Split("or");
	var ranges = rangeParts
		.Select(r => r.Split("-"))
		.Select(r => new LongRange(long.Parse(r[0]), long.Parse(r[1])))
		.ToList();

	return new Rule(parts[0], ranges);
}

Ticket ParseTicket(string line)
{
	return new Ticket(line.Split(",").Select(long.Parse).ToList());
}

record Rule(string Name, List<LongRange> ValidRanges)
{
	public bool IsValid(long value) => ValidRanges.Any(r => r.Contains(value));
}

record Ticket(List<long> Attributes)
{
	public long GetInvalidAttributes(List<Rule> rules) => Attributes
		.Where(a => rules.All(r => !r.IsValid(a)))
		.Sum();

	public List<(int position, long value)> GetAttributeIndex()
		=> Attributes.Select((a, i) => (i, a)).ToList();
}