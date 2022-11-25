namespace Lab5_NET;

public class FoatNormalForm
{
    private readonly List<List<char>> _fnf = new();

    public FoatNormalForm(char[] word, DependencyMatrix dependencies)
    {
        if (word.Length == 1)
        {
            PutOnLayer(word[0], 1);
            return;
        }

        var chars = word.Select(x => new WordElement(x, true)).ToArray();

        for (var i = 0; i < word.Length; i++)
        {
            if (chars[i].Used)
                continue;
            
            Mask(i, chars, dependencies);

            var newLayer = new List<char>() {chars[i].Name};
            chars[i].Used = true;

            for (var j = i + 1; j < word.Length; j++)
                if (chars[j].Mask && !chars[j].Used)
                {
                    Mask(j, chars, dependencies);
                    newLayer.Add(chars[j].Name);
                    chars[j].Used = true;
                }
            
            _fnf.Add(newLayer);
            ResetMask(chars);
        }
    }

    private void Mask(int startIndex, WordElement[] word, DependencyMatrix dependencies)
    {
        word[startIndex].Mask = false;

        for (var i = startIndex + 1; i < word.Length; i++)
            if (!word[i].Mask || dependencies.IsPairDepended(word[startIndex].Name, word[i].Name))
                Mask(i, word, dependencies);
    }
    private void ResetMask(WordElement[] word)
    {
        foreach (var w in word)
            w.Mask = true;
    }

    private void PutOnLayer(char ch, int layer)
    {
        if (layer > _fnf.Count)
            _fnf.Add(new ());
        
        _fnf[layer].Add(ch);
    }

    public string GetFnfText() =>
        $"FNF([w]) = ({string.Join(")(", _fnf.Select(x => string.Concat(x)))})";



    private record WordElement(char Name, bool Mask, bool Used = false)
    {
        public bool Mask { get; set; } = Mask;
        public bool Used { get; set; } = Used;
    }
}