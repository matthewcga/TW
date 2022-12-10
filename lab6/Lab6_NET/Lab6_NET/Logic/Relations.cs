using Lab6_NET.Models;

namespace Lab6_NET.Logic;


/// <summary>
/// Klasa pomocnicza wypisująca relacje zależności i niezależności
/// </summary>
public static class Relations
{
    /// <summary>
    /// Metdoa zwraca dwa stringi, relacji zależności  i niezależnośći
    /// </summary>
    /// <param name="alphabet">alfabety produkcji</param>
    /// <returns>relacja zależności, relacja niezależności</returns>
    public static (string d, string i) GetRelations(HashSet<Production> alphabet)
    {
        var array = alphabet.ToArray();
        List<(Production, Production)> d = new(), i = new();
        
        for (var a = 0; a < array.Length; a++)
        for (var b = a + 1; b < array.Length; b++)
            (array[a].CheckDependency(array[b]) ? d : i).Add((array[a], array[b]));
        
        return (
            $"D = sym{{\n{string.Join("\n", d.Select(x => $"[{x.Item1}, {x.Item2}]"))}\n}}",
            $"I = sym{{\n{string.Join("\n", i.Select(x => $"[{x.Item1}, {x.Item2}]"))}\n}}"
            );
    }
}