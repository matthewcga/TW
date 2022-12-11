using System.Text;

namespace Lab6_NET.Models;


/// <summary>
/// Klasa realizująca macierz kwadratową (z wektorem)
/// </summary>
public class Matrix2D : ICloneable
{
    public int Size { get; }
    private decimal[][] Matrix  { get; }
    
    
    /// <summary>
    /// Inicjalizuje pustą macierz
    /// </summary>
    /// <param name="size">rozmiar macierzy</param>
    public Matrix2D(int size)
    {
        Size = size;
        Matrix = new decimal[Size][];
        for (var i = 0; i < Size; i++)
            Matrix[i] = new decimal[Size + 1];
    }
    
    
    public decimal this[Cell cell]
    {
        get => Matrix[cell.Row][cell.Col];
        set => Matrix[cell.Row][cell.Col] = value;
    }
    
    
    public decimal this[int row, int col]
    {
        get => Matrix[row][col];
        set => Matrix[row][col] = value;
    }
    
    
    public override string ToString()
    {
        StringBuilder sb = new();
        for (var i = 0; i < Size; i++)
            sb.AppendLine(
                $"[ {string.Concat(Matrix[i][..Size].Select(x => $"{x, 10:####.0###}"))} | " +
                $"{Matrix[i][Size], 10:####.0###} ]");
        return sb.ToString().TrimEnd();
    }
    
    
    public object Clone()
    {
        var matrix = new Matrix2D(Size);
        for (var i = 0; i < Size; i++)
            Array.Copy(Matrix[i], 0, matrix.Matrix[i], 0, Size + 1);
        return matrix;
    }
}