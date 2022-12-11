﻿using Lab6_NET.Enums;
using Lab6_NET.Interfaces;
using Lab6_NET.Models;

namespace Lab6_NET.Solvers;

/// <summary>
/// Klasa tworzy listę operacji potrzebnych do przekształcenia macierzy do postaci górnej trójkątnej, nie modyfikuje macierzy
/// </summary>
public class MatrixSolverProductions : MatrixSolver, IPartialSolver
{
    public List<Production> Operations { get; } = new();
    
    
    public MatrixSolverProductions(Matrix2D matrix) : base(matrix) { }
    
    
    /// <summary>
    /// Tworzy listę operacji potrzebnych do przekształcenia macierzy do postaci górnej trójkątnej, nie modyfikuje macierzy
    /// </summary>
    public new void SolvePartially()
    {
        var pass = 0;
        for (var i = 0; i < Matrix.Size - 1; i++)
        for (var k = i + 1; k < Matrix.Size; k++, pass++)
        {
            Operations.Add(new (EOperation.A, pass, new (i, i), new (k, i)));

            for (var j = 0; j < Matrix.Size + 1; j++)
            {
                Operations.Add(new (EOperation.B, pass, new (k, j), Cell.Empty));
                Operations.Add(new (EOperation.C, pass, new (k, j), new (i,j)));
            }
        }
    }
}