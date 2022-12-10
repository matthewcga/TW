using System.Diagnostics;
using Lab6_NET.Logic;
using Lab6_NET.Models;
using Lab6_NET.Solvers;

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
                inFile = args[0],
                outFile = $"{Path.GetDirectoryName(inFile)}\\{Path.GetFileNameWithoutExtension(inFile)}_results.txt";
            var output = new OutputHelper(outFile);
            
            
            // serializacja macierzy
            var matrix = Serializer.SerializeMatrix(inFile);
            OutputHelper.ChangeSectionColor();
            output.PrintAndSaveToFile("\nWczytana macierz:", $"{matrix}");
            
            
            // stworzenie listy operacji potrzebnych do przekształcenia macierzy do postaci gónej trójkątnej
            var solver = new MatrixSolverOperations((Matrix2D)matrix.Clone());
            solver.SolvePartially();
            
            
            // wypisanie alfabetu
            var alphabet = solver.Operations.ToHashSet();
            OutputHelper.ChangeSectionColor();
            output.PrintAndSaveToFile("\nAlfabet produkcji:", Productions.GetAlphabet(alphabet));
            OutputHelper.ChangeSectionColor();
            output.PrintAndSaveToFile("\nSłowo:", Productions.GetWord(solver.Operations));

            
            // wypisanie relacji zależności i niezależności
            var (d, i) = Relations.GetRelations(alphabet);
            OutputHelper.ChangeSectionColor();
            output.PrintAndSaveToFile("\nRelacje zależności:", d);
            OutputHelper.ChangeSectionColor();
            output.PrintAndSaveToFile("\nRelacje niezależności:", i);

            
            // wypisanie postaci normalnej Foaty
            OutputHelper.ChangeSectionColor();
            output.PrintAndSaveToFile("\nPostać normalna Foaty:");
            var fnf = new NormalForm(solver.Operations);
            output.PrintAndSaveToFile(fnf.GetFnfText());
            
            
            // rozwiązanie macierzy
            OutputHelper.ChangeSectionColor();
            output.PrintAndSaveToFile("\nOczekiwany wynik:");
            var fullSolver = new MatrixSolverFull((Matrix2D)matrix.Clone());
            fullSolver.SolvePartially();
            output.PrintAndSaveToFile($"{fullSolver.Matrix}\n");
            fullSolver.FinishSolving();
            output.PrintAndSaveToFile($"{fullSolver.Matrix}");

            
            // rozwiązanie macierzy współbieżnie
            OutputHelper.ChangeSectionColor();
            output.PrintAndSaveToFile("\nOtrzymany (współbieżnie) wynik:");
            var asyncSolver = new MatrixSolverAsync((Matrix2D)matrix.Clone(), fnf);
            asyncSolver.SolvePartially();
            output.PrintAndSaveToFile($"{asyncSolver.Matrix}\n");
            asyncSolver.FinishSolving();
            output.PrintAndSaveToFile($"{asyncSolver.Matrix}");
            
            
            output.Dispose();
            OutputHelper.ChangeSectionColor();
            OutputHelper.Print($"\nWynik zapisano do: '{outFile}'\n");
            
            
            // generowanie grafu zeleżności
            OutputHelper.ChangeSectionColor();
            OutputHelper.GenerateFnfImage(outFile, fnf);
            
            
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            OutputHelper.Error(ex);
            Environment.Exit(1);
        }
    }
}