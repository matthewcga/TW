using Lab6_NET.Enums;
using Lab6_NET.Interfaces;
using Lab6_NET.Models;

namespace Lab6_NET.Solvers;

/// <summary>
/// Klasa tworzy listę operacji potrzebnych do przekształcenia macierzy do postaci gónej trójkątnej, nie modyfikuje macierzy
/// </summary>
public class MatrixSolverOperations : MatrixSolver, IPartialSolver
{
    public List<Production> Operations { get; } = new();
    
    
    public MatrixSolverOperations(Matrix2D matrix) : base(matrix) { }
    
    
    /// <summary>
    /// Tworzy listę operacji potrzebnych do przekształcenia macierzy do postaci gónej trójkątnej, nie modyfikuje macierzy
    /// </summary>
    public new void SolvePartially()
    {
        var pass = 0;
        for (var i = 0; i < Matrix.Size - 1; i++)
        for (var k = i + 1; k < Matrix.Size; k++, pass++)
        {
            Operations.Add(new Production(EOperation.A, new (i, i), new (k, i)));
            for (var j = 0; j < Matrix.Size + 1; j++)
            {
                Operations.Add(new Production(EOperation.B, new (i, j), Pass:pass));
                Operations.Add(new Production(EOperation.C, new (k, j), new (i,j)));
            }
        }
    }
}