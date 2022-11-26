using System.Text.RegularExpressions;

namespace Lab5_NET;

/// <summary>
/// Serializuje i trzyma dane wczytane z pliku wejścia
/// </summary>
public class DataSerializer
{
    private const string
        AlphabetPattern = @"{\s*([a-z],\s*)*[a-z]\s*}",                                                          // { a, b, c, d }
        WordPattern = @"[a-z]\s*=\s*(?<word>[a-z]+)",                                                            // w = abcdefg
        ProductionPattern = @"(?<name>[a-z])\)\s*(?<v0>[a-z])\s*\S*\s*\d*(?<v1>[a-z])?\s*\S\s*\d*(?<v2>[a-z])?"; // a) = 2x + 3y
    
    
    private readonly HashSet<char> _alphabet = new();
    private readonly List<char> _word = new();
    private readonly Dictionary<char, Production> _productions = new();

    
    public char[] Alphabet => _alphabet.ToArray();
    public char[] Word => _word.ToArray();
    public Dictionary<char, Production> Productions => _productions;

    
    /// <summary>
    /// Konstruktor który odczytuje plik danych, serializuje i waliduje je.
    /// </summary>
    /// <param name="path">ścieżka do pliku z danymi</param>
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

    
    /// <summary>
    /// Metoda próbuje ustawić alfabet.
    /// </summary>
    /// <param name="line">linijka pliku wejścia</param>
    /// <returns>zwraca prawdę jeżeli uda się serializować alfabet</returns>
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

    
    /// <summary>
    /// Metoda próbuje ustawić słowo.
    /// </summary>
    /// <param name="line">linijka pliku wejścia</param>
    /// <returns>zwraca prawdę jeżeli uda się serializować słowo, fałsz jeżeli linijka byłą pusta</returns>
    /// <exception cref="Exception">jeżeli po alfabecie jest niepusta linijka, która nie spełnia wzoru słowa, rzucony zostanie wyjątek</exception>
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

    
    /// <summary>
    /// Metoda która dodaje produkcje.
    /// </summary>
    /// <param name="line">linijka pliku wejścia</param>
    /// <exception cref="Exception">jeżeli podana została niepusta linijka, która nie spełnia wzoru produkcji, rzucony zostanie wyjątek</exception>
    private void AddProduction(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return;
        
        if (!Regex.IsMatch(line, ProductionPattern))
            throw new Exception($"Nieoczekiwany format produkcji!\nOczekiwano: 'a) = x + y'\nOtrzymano: '{line}'");

        var groups = new Regex(ProductionPattern).Match(line).Groups;
        char
            key = groups["name"].Value[0],                                                // dopasowywuje 'a' z 'a) : x = y + z'
            v0 = groups["v0"].Value[0];                                                   // dopasowywuje 'x' z 'a) : x = y + z'
        char?
            v1 = string.IsNullOrEmpty(groups["v1"].Value) ? null : groups["v1"].Value[0], // dopasowywuje 'y' z 'a) : x = y + z', lub null jeżeli puste
            v2 = string.IsNullOrEmpty(groups["v2"].Value) ? null : groups["v2"].Value[0]; // dopasowywuje 'z' z 'a) : x = y + z', lub null jeżeli puste

        if (!_alphabet.Contains(key))
            throw new Exception($"Alfabet nie posiada produkcji '{key}'!");

        if (_productions.ContainsKey(key))
            throw new Exception($"Słownik produkcji posiada już wpis dla '{key}'!");
        
        _productions.Add(key, new Production(v0, v1, v2));
    }

    
    /// <summary>
    /// Prosta walidacja słowa i produkcji.
    /// Inne rzeczy zapewnione mamy z Regex'a (np. niepusty słownik).
    /// </summary>
    /// <exception cref="Exception">Rzucony zostanie wyjątek z informacją jeżeli coś jest nie tak</exception>
    private void Validate()
    {
        if (!_word.Any())
            throw new Exception("Słowo jest puste lub nie zostało znalezione!");
        if (_productions.Count < _alphabet.Count)
            throw new Exception("Nie uzupełniono wszystkich produkcji alfabetu!");
    }

    
    /// <summary>
    /// Zwraca alfabet A, słowo w, produkcje.
    /// </summary>
    /// <returns>{A, w, produkcje}</returns>
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