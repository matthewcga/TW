using Lab6_NET.Logic;
using Lab6_NET.Models;
using Lab6_NET.Solvers;

namespace Lab6_NET;


/// <summary>
/// TO - Lab 6, Mateusz Cyganek.
/// Program Wypisuje alfabet, słowo, D, I, FNF i tworzy graf zależności na podstawie danych wejścia.
/// Ścieżkę do pliku z macierzą należy podać jako argument uruchomienia.
/// Program wygeneruje plik 'results' z wynikiem działania i 'img' z grafem.
/// </summary>
public static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            if (args.Length != 1)
                throw new Exception("Podaj ścieżkę do pliku z danymi jako argument uruchomienia programu!");


            var inFile = args[0];
            var (outFile, imgFile, slnFile) = OutputHelper.GetFilePaths(inFile);
            var output = new OutputHelper(outFile);
            
            
            // serializacja macierzy
            var matrix = Serializer.SerializeMatrix(inFile);
            OutputHelper.ChangeSectionColor();
            output.PrintAndWriteToFile("\nWczytana macierz:", $"{matrix}");
            
            
            // stworzenie listy operacji potrzebnych do przekształcenia macierzy do postaci górnej trójkątnej
            var solverOperations = new MatrixSolverProductions((Matrix2D)matrix.Clone());
            solverOperations.SolvePartially(); // zapisuje operacje do listy produkcji
            
    
            // wypisanie alfabetu
            var alphabet = solverOperations.Productions.ToHashSet();
            OutputHelper.ChangeSectionColor();
            output.PrintAndWriteToFile("\nAlfabet produkcji:", Productions.GetAlphabet(alphabet));
            OutputHelper.ChangeSectionColor();
            output.PrintAndWriteToFile("\nSłowo:", Productions.GetWord(solverOperations.Productions));

            
            // wypisanie relacji zależności i niezależności
            var (d, i) = Relations.GetRelations(alphabet);
            OutputHelper.ChangeSectionColor();
            output.PrintAndWriteToFile("\nRelacje zależności:", d);
            OutputHelper.ChangeSectionColor();
            output.PrintAndWriteToFile("\nRelacje niezależności:", i);


            // wypisanie postaci normalnej Foaty
            OutputHelper.ChangeSectionColor();
            output.PrintAndWriteToFile("\nPostać normalna Foaty:");
            var fnf = new NormalForm(solverOperations.Productions);
            output.PrintAndWriteToFile(fnf.GetFnfText());
          
            
            // rozwiązanie macierzy
            OutputHelper.ChangeSectionColor();
            output.PrintAndWriteToFile("\nOczekiwany wynik:");
            var fullSolver = new MatrixSolverFull((Matrix2D)matrix.Clone());
            fullSolver.SolvePartially();
            output.PrintAndWriteToFile($"{fullSolver.Matrix}\n");
            fullSolver.FinishSolving();
            output.PrintAndWriteToFile($"{fullSolver.Matrix}");
            
            
            // rozwiązanie macierzy współbieżnie
            OutputHelper.ChangeSectionColor();
            output.PrintAndWriteToFile("\nOtrzymany (współbieżnie) wynik:");
            var asyncSolver = new MatrixSolverAsync((Matrix2D)matrix.Clone(), fnf);
            asyncSolver.SolvePartially();
            output.PrintAndWriteToFile($"{asyncSolver.Matrix}\n");
            asyncSolver.FinishSolving();
            output.PrintAndWriteToFile($"{asyncSolver.Matrix}");


            output.Dispose();
            OutputHelper.ChangeSectionColor();
            OutputHelper.Print($"\nWynik zapisano do: '{outFile}'");
            
            
            output = new OutputHelper(slnFile);
            output.WriteToFile(asyncSolver.Matrix.ToShortString());
            output.Dispose();
            OutputHelper.ChangeSectionColor();
            OutputHelper.Print($"\nRozwiązanie zapisano do: '{slnFile}'\n");
            
            
            // generowanie grafu zależności
            OutputHelper.ChangeSectionColor();
            if (GraphHelper.GenerateFnfImage(imgFile, fnf))
                GraphHelper.ShowGraph(imgFile);
            else
                OutputHelper.Print("Niepowodzenie przy generowaniu obrazu!");

            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            OutputHelper.Error(ex);
            Environment.Exit(1);
        }
    }
}