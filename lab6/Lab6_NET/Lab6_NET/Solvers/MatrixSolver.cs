using Lab6_NET.Enums;
using Lab6_NET.Interfaces;
using Lab6_NET.Models;

namespace Lab6_NET.Solvers;

/// <summary>
/// Rozwiązuje macierz do postaci górnej trójkątnej
/// </summary>
public class MatrixSolver : IPartialSolver
{
    public Matrix2D Matrix { get; }
    protected readonly decimal[] Multipliers;


    protected MatrixSolver(Matrix2D matrix)
    {
        Matrix = matrix;
        Multipliers = new decimal[(Matrix.Size - 1) * (Matrix.Size - 1)];
    }
    
    
    /// <summary>
    /// Rozwiązuje macierz do postaci górnej trójkątnej
    /// </summary>
    public void SolvePartially()
    {
        var pass = 0;
        for (var i = 0; i < Matrix.Size - 1; i++)
        for (var k = i + 1; k < Matrix.Size; k++, pass++)
        {
            Invoke(new Production(EOperation.A, new (i, i), new (k, i), pass));
            
            for (var j = 0; j < Matrix.Size + 1; j++)
            {
                Invoke(new Production(EOperation.B, new (k, j), Cell.Empty, pass));
                Invoke(new Production(EOperation.C, new (k, j), new (i, j), pass));
            }
        }
    }


    protected void Invoke(Production production)
    {
        switch (production.Operation)
        {
            case EOperation.A: A(production); return;
            case EOperation.B: B(production); return;
            case EOperation.C: C(production); return;
            default: throw new ArgumentOutOfRangeException();
        }
    }


    /// <summary>
    /// Operacja A, zwraca wynik dzielenia komórki 1 przez komórkę 2.
    /// "znalezienie mnożnika dla wiersza i, do odejmowania go od k-tego wiersza"
    /// </summary>
    /// <param name="production">produkcja z parametrami operacji</param>
    /// <returns>wynik dzielenia komórki 1 przez komórkę 2</returns>
    protected void A(Production production)
    { Multipliers[production.Pass] = Matrix[production.Cell1] / Matrix[production.Cell2]; }


    /// <summary>
    /// Operacja B, mnoży komórkę 1 przez podaną wartość
    /// "pomnożenie j-tego elementu wiersza i przez mnożnik - do odejmowania od k-tego wiersza,"
    /// </summary>
    /// <param name="production">produkcja z parametrami operacji</param>
    protected void B(Production production)
    { Matrix[production.Cell1] *= Multipliers[production.Pass]; }


    /// <summary>
    /// Operacja C, odejmuje od komórki 1 komórkę 2
    /// </summary>
    /// <param name="production">produkcja z parametrami operacji</param>
    protected void C(Production production)
    { Matrix[production.Cell1] -= Matrix[production.Cell2]; }
}