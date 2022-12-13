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
        
        // po każdej produkcji
        for (var i = 0; i < word.Count; i++)               
        {
            // pomiń jeżeli już została wykorzystana
            if (elements[i].Used)                 
                continue;
            
            // wstawiamy element do poziomu FNF
            MarkUsed(elements, i, layer, passesA, passesB); 
            
            // dla wszystkich kolejnych produkcji
            for (var j = i + 1; j < word.Count; j++)
                // jeżeli można wykonać produkcje j współbieżnie z i
                if (IsConcurrent(elements, j, layer, passesA, passesB))
                    // dodajemy produkcje j do poziomu FNF
                    MarkUsed(elements, j, layer, passesA, passesB);

            // wstawiamy wygenerowany poziom do FNF
            Fnf.Add(layer.Select(x => x.Production).ToList());
            // czyścimy warstwę roboczą
            layer.Clear();  
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
    public StringBuilder GetGraphNodesCsv()
    {
        StringBuilder sb = new();
        for (var i = 0; i < Fnf.Count - 1; i++)
        {
            foreach (var current in Fnf[i])
                sb.Append(string.Concat(current.Operation switch
                {
                    EOperation.A => Fnf[i + 1]
                        .Where(x => x.Pass == current.Pass)
                        .Select(x => $"{current}.{x};"),
                    EOperation.B => Fnf[i + 1]
                        .Where(x => x.Pass == current.Pass && x.Cell1 == current.Cell1)
                        .Select(x => $"{current}.{x};"),
                    EOperation.C => Fnf[i + 1]
                        .Select(x => $"{current}.{x};"),
                    _ => throw new ArgumentOutOfRangeException()
                }));
            sb.AppendLine(";");
        }
        return sb;
    }
}