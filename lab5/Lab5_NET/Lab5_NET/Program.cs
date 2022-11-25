namespace Lab5_NET;

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
            
            var serializer = new DataSerializer(inFile);
            var dependencies = new DependencyMatrix(serializer.Alphabet, serializer.Productions);
            var fnf = new FoatNormalForm(serializer.Word, dependencies);
            
            using var oh = new OutputHelper(outFile);
            {
                oh.PrintAndSaveToFile(ConsoleColor.Cyan, serializer.GetDataText());
                oh.PrintAndSaveToFile(ConsoleColor.Yellow, dependencies.GetRelationsText());
                oh.PrintAndSaveToFile(ConsoleColor.Magenta, fnf.GetFnfText());
            }
            
            OutputHelper.GenerateImage(outFile);
            Console.WriteLine($"Ukończono! Wynik zapisano do: '{outFile}'.");
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            OutputHelper.Print(ConsoleColor.Red, ex.Message);
            Environment.Exit(1);
        }
    }
}