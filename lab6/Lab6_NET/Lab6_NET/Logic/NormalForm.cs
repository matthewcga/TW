using System.Text;
using Lab6_NET.Enums;
using Lab6_NET.Models;

namespace Lab6_NET.Logic;

/// <summary>
/// klasa przekształcająca słowo do FNF
/// </summary>
public class NormalForm
{
    public List<List<Production>> Fnf { get; }= new();
    
    
    /// <summary>
    /// Konstruktor klasy FNF, tworzy FNF na podstawie słowa
    /// </summary>
    /// <param name="word">słowo</param>
    public NormalForm(List<Production> word)
    {
        List<(Production Production, bool Used)> layer = new();
        HashSet<int> passesA = new(), passesB = new();
        (Production Production, bool Used)[] elements = word.Select(x => (x, false)).ToArray();
        
        for (var i = 0; i < word.Count; i++)                                    // po każdej produkcji
        {
            if (elements[i].Used)                                               // pomiń jeżeli już została wykorzystana
                continue;
            
            MarkUsed(elements, i, layer, passesA, passesB);                     // wstawiamy element do poziomu FNF
            
            for (var j = i + 1; j < word.Count; j++)                         // dla wszystkich kolejnych produkcji
                if (IsConcurrent(elements, j, layer, passesA, passesB))         // jeżeli można wykonać produkcje j współbieżnie z i
                    MarkUsed(elements, j, layer, passesA, passesB);             // dodajemy produkcje j do poziomu FNF

            Fnf.Add(layer.Select(x => x.Production).ToList()); // wstawiamy wygenerowany poziom do FNF
            layer.Clear();                                                      // czyścimy warstwę roboczą
        }
    }


    private bool IsConcurrent(
        (Production Production, bool Used)[] elements,
        int i,
        List<(Production Production, bool Used)> layer,
        HashSet<int> passesA,
        HashSet<int> passesB
    )
    {
        if (elements[i].Used)
            return false;
        if (elements[i].Production.Operation == EOperation.B && !passesA.Contains(elements[i].Production.Pass))
            return false;
        if (elements[i].Production.Operation == EOperation.C && !passesB.Contains(elements[i].Production.Pass))
            return false;
        if (layer.Any(x => elements[i].Production.IsDependentOn(x.Production)))
            return false;
        return true;
    }
    
    
    private void MarkUsed(
        (Production Production, bool Used)[] elements,
        int i,
        List<(Production Production, bool Used)> layer,
        HashSet<int> passesA,
        HashSet<int> passesB
    )
    {
        elements[i].Used = true;
        layer.Add(elements[i]);
            
        if (elements[i].Production.Operation == EOperation.A)
            passesA.Add(elements[i].Production.Pass);
        else if (elements[i].Production.Operation == EOperation.B)
            passesB.Add(elements[i].Production.Pass);
    }
    
    
    public string GetFnfText()
    { return $"FNF([w]) = \n[{string.Join("]\n[", Fnf.Select(x => string.Join(" ", x)))}]"; }


    /// <summary>
    /// Metoda pomocnicza dla tworzenia grafu
    /// Przekazuje plik csv do skryptu .py z FNF
    /// </summary>
    public StringBuilder GetCsv()
    {
        StringBuilder sb = new();
        foreach (var layer in Fnf)
        {
            foreach (var production in layer)
                sb.Append($"{production.Operation}.{production.Cell1}.{production.Cell2};");
            sb.Append('\n');
        }
        return sb;
    }
}