using Lab6_NET.Interfaces;
using Lab6_NET.Models;

namespace Lab6_NET.Solvers;

/// <summary>
/// Rozwiązuje macierz do postaci górnej trójkątnej
/// </summary>
public class MatrixSolver : IPartialSolver
{
    public Matrix2D Matrix { get; }


    protected MatrixSolver(Matrix2D matrix)
    { Matrix = matrix; }
    
    
    /// <summary>
    /// Rozwiązuje macierz do postaci górnej trójkątnej
    /// </summary>
    public void SolvePartially()
    {
        for (var i = 0; i < Matrix.Size - 1; i++)
        for (var k = i + 1; k < Matrix.Size; k++)
        {
            var multiplayer = A(new (i,i), new (k, i));

            for (var j = 0; j < Matrix.Size + 1; j++)
            {
                B(new (k, j), multiplayer);
                C(new (k, j), new (i, j));
            }
        }
    }


    /// <summary>
    /// Operacja A, zwraca wynik dzielenia komórki 1 przez komórkę 2
    /// </summary>
    /// <param name="cell1">komórka 1</param>
    /// <param name="cell2">komórka 2</param>
    /// <returns>wynik dzielenia komórki 1 przez komórkę 2</returns>
    protected decimal A(Cell cell1, Cell cell2)
    { return Matrix[cell1.Row, cell1.Col] / Matrix[cell2.Row, cell2.Col]; }


    /// <summary>
    /// Operacja B, mnoży komórkę 1 przez podaną wartość
    /// </summary>
    /// <param name="cell1">komórka 1</param>
    /// <param name="value">mnożnik</param>
    protected void B(Cell cell1, decimal value)
    { Matrix[cell1.Row, cell1.Col] *= value; }


    /// <summary>
    /// Operacja C, odejmuje od komórki 1 komórkę 2
    /// </summary>
    /// <param name="cell1">komórka 1</param>
    /// <param name="cell2">komórka 2</param>
    protected void C(Cell cell1, Cell cell2)
    { Matrix[cell1.Row, cell1.Col] -= Matrix[cell2.Row, cell2.Col]; }
}