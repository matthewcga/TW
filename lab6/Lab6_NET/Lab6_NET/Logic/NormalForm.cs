using System.Text;
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
        var elements = word.Select(x => new WordElement(x, true)).ToArray();

        for (var i = 0; i < word.Count; i++)
        {
            if (elements[i].Used) continue;                             // jeżeli już uśyliśmy to pomijamy
            Mask(i, elements);                                          // sprawdzamy które elementy są zależne
            var newLayer = new List<Production>() { elements[i].Name }; // dodajemy obecne słowo
            elements[i].Used = true;                                    // i ustawiamy je na użyte

            for (var j = i + 1; j < word.Count; j++)                 // dla wszystkich kolejnych elementów słowa
                if (elements[j].Mask && !elements[j].Used)              // sprawdzamy czy są niezamaskowane i nie użyte
                {
                    Mask(j, elements);                                  // wykonujemy maskowanie dla elementów które mogą być zależne od obecnego
                    newLayer.Add(elements[j].Name);                     // dodajemy element
                    elements[j].Used = true;                            // i ustawiamy na użyte
                }
            
            Fnf.Add(newLayer);
            ResetMask(elements);
        }
    }
    
    
    private void Mask(int startIndex, WordElement[] word)
    {
        word[startIndex].Mask = false;
        for (var i = startIndex + 1; i < word.Length; i++)
            if (!word[i].Mask || word[startIndex].Name.CheckDependency(word[i].Name))
                Mask(i, word);
    }


    private void ResetMask(WordElement[] word)
    {
        foreach (var w in word)
            w.Mask = true;
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
    
    
    private record WordElement(Production Name, bool Mask)
    {
        public Production Name { get; } = Name;
        public bool Mask { get; set; } = Mask;
        public bool Used { get; set; }
    }
}