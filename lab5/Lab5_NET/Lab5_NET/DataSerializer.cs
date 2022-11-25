using System.Text.RegularExpressions;

namespace Lab5_NET;

public class DataSerializer
{
    private const string
        AlphabetPattern = @"{([a-z],)*[a-z]}",
        WordPattern = @"w\s*=\s*(?<word>[a-z]+)",
        ProductionPattern = @"(?<name>[a-z])\)\s*(?<v0>[a-z])\s*\W\s*\d*(?<v1>[a-z])?\s*\W\s*\d*(?<v2>[a-z])?";
    
    private readonly HashSet<char> _alphabet = new(), _variables = new();
    private readonly List<char> _word = new();
    private readonly Dictionary<char, Production> _productions = new();

    public char[] Alphabet => _alphabet.ToArray();
    public char[] Word => _word.ToArray();
    public Dictionary<char, Production> Productions => _productions;

    public DataSerializer(string path)
    {
        using var lines = File.ReadLines(path).GetEnumerator();
        {
            while (lines.MoveNext())
                if (TrySetAlphabet(lines.Current))
                    break;

            while (lines.MoveNext())
                if (TrySetWord(lines.Current))
                    break;

            while (lines.MoveNext())
                AddProduction(lines.Current);

            Validate();
        }
    }

    private bool TrySetAlphabet(string line)
    {
        if (!Regex.IsMatch(line, AlphabetPattern))
            return false;

        line = new Regex(@"[^a-z]")
            .Replace(line, string.Empty);

        foreach (var ch in line)
            _alphabet.Add(ch);

        return true;
    }

    private bool TrySetWord(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return false;
        
        if (!Regex.IsMatch(line, WordPattern))
            throw new Exception($"Nieoczekiwany format słowa!\nOczekiwano: 'w = abcd', Otrzymano:  '{line}'");

        line = new Regex(WordPattern)
            .Match(line).Groups["word"].Value;
        
        foreach (var ch in line)
            if (_alphabet.Contains(ch))
                _word.Add(ch);
            else
                throw new Exception($"Produkcja '{ch}' nie należy do alfabetu!");

        if (!_word.Any())
            throw new Exception("Słowo jest puste!");

        return true;
    }

    private void AddProduction(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return;
        
        if (!Regex.IsMatch(line, ProductionPattern))
            throw new Exception($"Nieoczekiwany format produkcji!\nOczekiwano: 'a) = x + y'\nOtrzymano: '{line}'");

        var groups = new Regex(ProductionPattern)
            .Match(line).Groups;

        char
            key = groups["name"].Value[0],
            v0 = groups["v0"].Value[0];
        char?
            v1 = string.IsNullOrEmpty(groups["v1"].Value) ? null : groups["v1"].Value[0],
            v2 = string.IsNullOrEmpty(groups["v2"].Value) ? null : groups["v2"].Value[0];

        foreach (var v in new []{v0, v1, v2})
            if (v.HasValue && !_variables.Contains(v.Value))
                _variables.Add(v.Value);
        
        if (!_alphabet.Contains(key))
            throw new Exception($"Alfabet nie posiada produkcji '{key}'!");

        if (_productions.ContainsKey(key))
            throw new Exception($"Słownik produkcji posiada już wpis dla '{key}'!");
        
        _productions.Add(key, new Production(v0, v1, v2));
    }

    private void Validate()
    {
        if (!_word.Any())
            throw new Exception("Słowo jest puste lub nie zostało znalezione!");
        if (_productions.Count < _alphabet.Count)
            throw new Exception("Nie uzupełniono wszystkich produkcji alfabetu!");
    }

    public string[] GetDataText()
    {
        return new[]
        {
            $"A = {{{string.Join(", ", _alphabet)}}}",
            $"w = {string.Join(string.Empty, _word)}",
            $"{string.Join("\n", _productions.Select(x => $"{x.Key}) {x.Value}"))}"
        };
    }
}