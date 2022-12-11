using Lab6_NET.Models;

namespace Lab6_NET.Logic;


/// <summary>
/// Klasa pomocnicza wypisująca relacje zależności i niezależności
/// </summary>
public static class Relations
{
    /// <summary>
    /// Metoda zwraca dwa stringi, relacji zależności  i niezależności
    /// </summary>
    /// <param name="alphabet">alfabety produkcji</param>
    /// <returns>relacja zależności, relacja niezależności</returns>
    public static (string d, string i) GetRelations(HashSet<Production> alphabet)
    {
        var array = alphabet.ToArray();
        List<(Production, Production)> d = new(), i = new();
        
        foreach (var p1 in array)
            foreach (var p2 in array)
                (p1.IsDependentOn(p2) ? d : i).Add((p1, p2));

        return (
            $"D = sym{{\n{string.Join("\n", d.Select(x => $"[{x.Item1}, {x.Item2}]"))}\n}}",
            $"I = sym{{\n{string.Join("\n", i.Select(x => $"[{x.Item1}, {x.Item2}]"))}\n}}");
    }
}