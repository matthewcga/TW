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


    /// <summary>
    /// Rozwiązuje macierz do postaci górnej trójkątnej wielowątkowo
    /// </summary>
    public MatrixSolverAsync(Matrix2D matrix, NormalForm fnf) : base(matrix)
    { _fnf = fnf.Fnf; }
    

    /// <summary>
    /// Rozwiązuje macierz do postaci górnej trójkątnej wielowątkowo, każdą operacje na oddzielnym task'u
    /// </summary>
    public new void SolvePartially()
    {
        // dla każdego poziomu FNF który zawiera operacje od siebie niezależne
        foreach (var level in _fnf)
            // wywołuje i oczekuje wykonania wszystkich operacji danego poziomu
            Task.WaitAll(level.Select(Invoke).ToArray());
    }

    private new async Task Invoke(Production production)
    { await Task.Run(() => base.Invoke(production)); }
}