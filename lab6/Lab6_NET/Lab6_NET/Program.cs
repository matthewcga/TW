using Lab6_NET.Logic;

namespace Lab6_NET;

/// <summary>
/// TO - Lab 6, Mateusz Cyganek.
/// </summary>
public static class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length != 1)
                throw new Exception("Podaj ścieżkę do pliku z danymi jako argument uruchomienia programu!");

            string
                inFile = args[0];
                //outFile = $"{Path.GetDirectoryName(inFile)}\\{Path.GetFileNameWithoutExtension(inFile)}_results.txt";
            
            var matrix = new MatrixSerializer(inFile).SerializeMatrix();
            OutputHelper.Print(ConsoleColor.Magenta, $"{matrix}");
            matrix.TrySolve();
            OutputHelper.Print(ConsoleColor.Cyan, $"{matrix}");
            OutputHelper.Print(ConsoleColor.Yellow, String.Join("\n", matrix.Operations));
            
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            OutputHelper.Print(ConsoleColor.Red, ex.Message);
            Environment.Exit(1);
        }
    }
}