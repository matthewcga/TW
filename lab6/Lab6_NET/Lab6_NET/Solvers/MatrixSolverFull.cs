using Lab6_NET.Interfaces;
using Lab6_NET.Models;

namespace Lab6_NET.Solvers;

/// <summary>
/// Rozwiązuje macierz do postaci jednostkowej
/// </summary>
public class MatrixSolverFull : MatrixSolver, IFullSolver
{
    public MatrixSolverFull(Matrix2D matrix) : base(matrix) { }
    
    
    /// <summary>
    /// Rozwiązuje macierz do postaci jednostkowej
    /// Musi zostać wywołany na macierzy w postaci górnej trójkątnej (odziedziczone)
    /// </summary>
    public void FinishSolving() {
        for (var i = Matrix.Size - 1; i >= 0; i--)
        {
            var multiplayer = A(new (i, i));
            B(new (i, Matrix.Size), multiplayer);
            B(new (i, i), multiplayer);
            for (var k = i - 1; k >= 0; k--)
            {
                C(new (k, Matrix.Size), new (k, i));
                C(new (k, i), new (k, i));
            }
        }
    }
    
    
    /// <summary>
    /// Operacja A, zwraca odwrotność komórki
    /// </summary>
    /// <param name="cell1">przekazana komórka</param>
    /// <returns>odwrotność komórki</returns>
    private decimal A(Cell cell1)
    { return 1.0m / Matrix[cell1]; }
}