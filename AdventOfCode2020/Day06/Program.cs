using Utils;

var groups = File.ReadAllLines("input.txt")
	.SplitByPredicate(string.IsNullOrEmpty)
	.ToList();

var uniqueAnswers = groups.Select(GetUniqueAnswerCount).ToList();
Console.WriteLine("1: " + uniqueAnswers.Sum());

var commonAnswers = groups.Select(GetCommonAnswerCount).ToList();
Console.WriteLine("2: " + commonAnswers.Sum());

int GetUniqueAnswerCount(List<string> answers)
{
	return answers.SelectMany(l => l).ToHashSet().Count();
}

int GetCommonAnswerCount(List<string> answers)
{
	var groups = answers.Select(x => x.ToHashSet());
	var set = groups.First();

	foreach (var answer in answers)
	{
		set = set.Intersect(answer).ToHashSet();
	}

	return set.Count;
}