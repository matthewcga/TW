using Lab6_NET.Enums;
using Lab6_NET.Interfaces;
using Lab6_NET.Logic;
using Lab6_NET.Models;

namespace Lab6_NET.Solvers;

/// <summary>
/// Rozwiązuje macierz do postaci górnej trójkątnej wielowątkowo
/// </summary>
public class MatrixSolverAsync : MatrixSolverFull, IFullSolver
{
    private readonly List<List<Production>> _fnf;
    private readonly decimal[] _multipliers;


    /// <summary>
    /// Rozwiązuje macierz do postaci górnej trójkątnej wielowątkowo
    /// </summary>
    public MatrixSolverAsync(Matrix2D matrix, NormalForm fnf) : base(matrix)
    {
        _fnf = fnf.Fnf;
        _multipliers = new decimal[(Matrix.Size - 1) * (Matrix.Size - 1)];
    }
    

    /// <summary>
    /// Rozwiązuje macierz do postaci górnej trójkątnej wielowątkowo, każdą operacje na oddzielnym task'u
    /// </summary>
    public new void SolvePartially()
    {
        foreach (var level in _fnf)                // dla każdego poziomu FNF który zawiera operacje od siebie niezależne
            Task.WaitAll(level.Select(GetTask).ToArray()); // wywołuje i oczekuje wykonania wszystkich operacji danego poziomu
    }
    
    
    /// <summary>
    /// Zwraca Task dla podanej produkcji
    /// </summary>
    /// <param name="production">produkcja zawierająca informacje o typie operacji i komórkach na których zachodzi</param>
    /// <returns>Task do wykonania</returns>
    private Task GetTask(Production production)
    {
        return production.Operation switch
        {
            EOperation.A => Task.Run(() => A(production.Cell1, production.Cell2, production.Pass)),
            EOperation.B => Task.Run(() => B(production.Cell1, _multipliers[production.Pass])),
            EOperation.C => Task.Run(() => C(production.Cell1, production.Cell2)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }


    /// <summary>
    /// operacja A pod pracę asynchroniczną
    /// </summary>
    /// <param name="cell1">komórka 1</param>
    /// <param name="cell2">komórka 2</param>
    /// <param name="pass">index</param>
    private void A(Cell cell1, Cell cell2, int pass)
    { _multipliers[pass] = A(cell1, cell2); }
}