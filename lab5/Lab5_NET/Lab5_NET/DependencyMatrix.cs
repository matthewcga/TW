namespace Lab5_NET;

public class DependencyMatrix
{
    private readonly char[] _alphabet; 
    private readonly HashSet<(char, char)> _matrix = new();

    public DependencyMatrix(char[] alphabet, IReadOnlyDictionary<char, Production> productions)
    {
        _alphabet = alphabet;
        
        for (var a = 0; a < _alphabet.Length; a++)
        for (var b = a; b < _alphabet.Length; b++)
        {
            char k1 = _alphabet[a], k2 = _alphabet[b];
            
            if (productions[k1].CheckDependency(productions[k2]))
                AddDependency(k1, k2);
        }
    }

    private (List<(char, char)> d, List<(char, char)> i) GetRelations()
    {
        List<(char, char)> d = new(), i = new();
        
        for (var a = 0; a < _alphabet.Length; a++)
        for (var b = a; b < _alphabet.Length; b++)
        {
            char k1 = _alphabet[a], k2 = _alphabet[b];

            if (IsPairDepended(k1, k2)) d.Add((k1, k2));
            else i.Add((k1, k2));
        }
        
        return (d, i);
    }

    private void AddDependency(char a, char b)
    {
        if (!IsPairDepended(a, b))
            _matrix.Add((a, b));
    }

    public bool IsPairDepended(char a, char b) =>
        a == b || _matrix.Contains(a < b ? (a, b) : (b, a));

    public string[] GetRelationsText()
    {
        var (d, i) = GetRelations();
        return new []{
            $"D = sym{{{string.Join(", ", d.Select(x => $"({x.Item1}, {x.Item2})"))}}}",
            $"I = sym{{{string.Join(", ", i.Select(x => $"({x.Item1}, {x.Item2})"))}}}"
        };
    }
}