namespace Lab5_NET;

/// <summary>
/// Klasa zajmująca się wszystkim co związane z FNF
/// </summary>
public class FoatsNormalForm
{
    /// <summary>
    /// fnf zapisana jako lista list z elementami np: '((ab)(cd)(ef))'
    /// </summary>
    private readonly List<List<char>> _fnf = new();

    
    /// <summary>
    /// Konstruktor, który buduje FNF na podstawie podanego słowa i macierzy zależności.
    /// </summary>
    /// <param name="word"></param>
    /// <param name="dependencies"></param>
    public FoatsNormalForm(char[] word, DependencyMatrix dependencies)
    {
        var chars = word.Select(x => new WordElement(x, true)).ToArray();

        for (var i = 0; i < word.Length; i++)
        {
            if (chars[i].Used) continue;                     // jeżeli już uśyliśmy to pomijamy
            
            Mask(i, chars, dependencies);                    // sprawdzamy które elementy są zależne

            var newLayer = new List<char>() {chars[i].Name}; // dodajemy obecne słowo
            chars[i].Used = true;                            // i ustawiamy je na użyte

            for (var j = i + 1; j < word.Length; j++)     // dla wszystkich kolejnych elementów słowa
                if (chars[j].Mask && !chars[j].Used)         // sprawdzamy czy są niezamaskowane i nie użyte
                {
                    Mask(j, chars, dependencies);            // wykonujemy maskowanie dla elementów które mogą być zależne od obecnego
                    newLayer.Add(chars[j].Name);             // dodajemy element
                    chars[j].Used = true;                    // i ustawiamy na użyte
                }
            
            _fnf.Add(newLayer);
            ResetMask(chars);
        }
    }

    
    /// <summary>
    /// Metoda rekurencyjnie ustawia maske na elementach słowa które sprawdziliśmy że są zależne od słowa pod startIndex.
    /// </summary>
    /// <param name="startIndex">element słowa od którego zaczynamy</param>
    /// <param name="word">elementy słowa</param>
    /// <param name="dependencies">macierz zależności</param>
    private void Mask(int startIndex, WordElement[] word, DependencyMatrix dependencies)
    {
        word[startIndex].Mask = false;

        for (var i = startIndex + 1; i < word.Length; i++)
            if (!word[i].Mask || dependencies.IsPairDepended(word[startIndex].Name, word[i].Name))
                Mask(i, word, dependencies);
    }
    
    
    /// <summary>
    /// Resetuje maski informujące o tym czy produkcja jest zależna od tej którą teraz sprawdzamy.
    /// </summary>
    /// <param name="word">elementy słowa</param>
    private void ResetMask(WordElement[] word)
    {
        foreach (var w in word)
            w.Mask = true;
    }


    /// <summary>
    /// Wypisuje FNF w odpowiedniej postaci.
    /// </summary>
    /// <returns>FNF w odpowiedniej postaci</returns>
    public string GetFnfText() =>
        $"FNF([w]) = ({string.Join(")(", _fnf.Select(x => string.Concat(x)))})";


    /// <summary>
    /// Rekord pomocniczy dla generowania FNF.
    /// </summary>
    /// <param name="Name">nazwa produkcji, odpowiada 'a' z a) x = y + z</param>
    /// <param name="Mask">Czy w jest niezależne od innych produkcji</param>
    /// <param name="Used">Czy już znajduje się w grafie</param>
    private record WordElement(char Name, bool Mask, bool Used = false)
    {
        public bool Mask { get; set; } = Mask;
        public bool Used { get; set; } = Used;
    }
}