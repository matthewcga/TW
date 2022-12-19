using System.Text;
using Lab6_NET.Enums;
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
    public void FinishSolving()
    {
        var pass = 0;
        for (var i = Matrix.Size - 1; i >= 0; i--, pass++)
        {
            A(new Production(EOperation.A,new (i, i), Cell.Empty, pass));
            B(new Production(EOperation.B, new (i, Matrix.Size), Cell.Empty, pass));
            B(new Production(EOperation.B, new (i, i), Cell.Empty, pass));
            for (var k = i - 1; k >= 0; k--)
            {
                C(new Production(EOperation.C, new (k, Matrix.Size), new (k, i), pass));
                C(new Production(EOperation.C, new (k, i), new (k, i), pass));
            }
        }
    }


    /// <summary>
    /// Operacja A, zwraca odwrotność komórki
    /// </summary>
    /// <param name="production">produkcja z parametrami operacji</param>
    /// <returns>odwrotność komórki</returns>
    private new void A(Production production)
    { Multipliers[production.Pass] =  1.0m / Matrix[production.Cell1]; }
}