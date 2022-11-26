namespace Lab5_NET;

/// <summary>
/// TO - Lab 5, Mateusz Cyganek.
/// Program Wypisuje alfabet, słowo, D, I, FNF i tworzy graf zależności na podstawie danych wejścia.
/// Ścieżkę do pliku należy podać jako argument uruchomienia.
/// Program wygeneruje plik 'results' z wynikiem działania i 'img' z grafem.
/// </summary>
public static class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length != 1)
                throw new Exception("Podaj ścieżkę do pliku z danymi jako argument uruchomienia programu!");
            if (!File.Exists(args[0]))
                throw new Exception("Podano Błędną ścieżkę do pliku!");
            
            string
                inFile = args[0],
                outFile = $"{Path.GetDirectoryName(inFile)}\\{Path.GetFileNameWithoutExtension(inFile)}_results.txt";
            
            var serializer = new DataSerializer(inFile);                                         // stwórz alfabet słowo i preodukcje na podstawie pliku
            var dependencies = new DependencyMatrix(serializer.Productions);                     // stwórz macierz zależności na podstawie produkcji
            var fnf = new FoatsNormalForm(serializer.Word, dependencies);                        // stwórz FNF na podstawie słowa i macierzy zależności
            
            var helper = new OutputHelper(outFile);
            helper.PrintAndSaveToFile(ConsoleColor.Cyan, serializer.GetDataText());          // wypisz alfabet i słowo
            helper.PrintAndSaveToFile(ConsoleColor.Yellow, dependencies.GetRelationsText()); // wypisz zależności i niezależności
            helper.PrintAndSaveToFile(ConsoleColor.Magenta, fnf.GetFnfText());         // wypisz FNF
            helper.Dispose();

            OutputHelper.Print(ConsoleColor.Blue, $"Wynik zapisano do: '{outFile}'.");
            OutputHelper.GenerateImage(outFile);                                                 // generuj graf zależności
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            OutputHelper.Print(ConsoleColor.Red, ex.Message);
            Environment.Exit(1);
        }
    }
}