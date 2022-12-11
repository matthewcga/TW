using Lab6_NET.Models;

namespace Lab6_NET.Logic;

public static class Productions
{
    public static string GetAlphabet(HashSet<Production> alphabet)
    { return $"A = {{\n{string.Join(",\n", alphabet)}\n}}"; }


    public static string GetWord(List<Production> word)
    { return $"w = \n{string.Join("\n", word)}"; }
}