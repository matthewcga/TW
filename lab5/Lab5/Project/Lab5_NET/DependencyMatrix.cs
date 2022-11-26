namespace Lab5_NET;

/// <summary>
/// Klasa budująca macierz zależności na podstawie produkcji
/// </summary>
public class DependencyMatrix
{
    private readonly char[] _alphabet;
    /// <summary>
    /// Macierz zależności (tak naprawdę realizacja pod macierze rzadkie poprzez Set).
    /// </summary>
    private readonly HashSet<(char, char)> _matrix = new();

    
    /// <summary>
    /// Konstruktor budujący macierz zależności.
    /// </summary>
    /// <param name="productions">produkcje z pliku wejścia</param>
    public DependencyMatrix(IReadOnlyDictionary<char, Production> productions)
    {
        _alphabet = productions.Keys.ToArray();
        
        for (var a = 0; a < _alphabet.Length; a++)
        for (var b = a; b < _alphabet.Length; b++)
        {
            char k1 = _alphabet[a], k2 = _alphabet[b];
            
            if (productions[k1].CheckDependency(productions[k2]))
                AddDependency(k1, k2);
        }
    }
    

    /// <summary>
    /// Zwraca relacje zależności i niezależności w postaci list.
    /// </summary>
    /// <returns>(D, I)</returns>
    private (List<(char, char)> d, List<(char, char)> i) GetRelations()
    {
        List<(char, char)> d = new(), i = new();
        
        for (var a = 0; a < _alphabet.Length; a++)
        for (var b = a + 1; b < _alphabet.Length; b++)
        {
            char k1 = _alphabet[a], k2 = _alphabet[b];

            if (IsPairDepended(k1, k2)) d.Add((k1, k2));
            else i.Add((k1, k2));
        }
        
        return (d, i);
    }

    
    /// <summary>
    /// Dodaje zależność, dodatkowo upewnia się że zostanie dodana w odpowiednej kolejności.
    /// </summary>
    /// <param name="a">lewa produkcja</param>
    /// <param name="b">prawa produkcja</param>
    private void AddDependency(char a, char b)
    {
        if (!IsPairDepended(a, b))
            _matrix.Add((a, b));
    }

    
    /// <summary>
    /// Zwraca informacje czy para produkcji jest od siebie zależna na podstawie macierzy zależności.
    /// </summary>
    /// <param name="a">pierwsza produkcja</param>
    /// <param name="b">druga produkcja </param>
    /// <returns>prawda jeżeli zależne</returns>
    public bool IsPairDepended(char a, char b) =>
        a == b || _matrix.Contains(a < b ? (a, b) : (b, a));

    
    /// <summary>
    /// Zwraca nam relacje zależności i niezależności na podstawie macierzy zależności.
    /// </summary>
    /// <returns>{D, I}</returns>
    public string[] GetRelationsText()
    {
        var (d, i) = GetRelations();
        return new []{
            $"D = sym{{{string.Join(", ", d.Select(x => $"({x.Item1}, {x.Item2})"))}}}",
            $"I = sym{{{string.Join(", ", i.Select(x => $"({x.Item1}, {x.Item2})"))}}}"
        };
    }
}