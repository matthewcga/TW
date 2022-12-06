using Lab6_NET.Models;

namespace Lab6_NET.Logic;


public class MatrixSerializer
{
    private readonly string file;

    public MatrixSerializer(string path)
    {
        if (!File.Exists(path))
            throw new Exception("Podano Błędną ścieżkę do pliku!");
        file = path;
    }
    
    public Matrix2D SerializeMatrix()
    {
        Matrix2D matrix;
        using var lines = File.ReadLines(file).GetEnumerator();
        {
            lines.MoveNext();
            matrix = new Matrix2D(int.Parse(lines.Current));

            for (var i = 0; i < matrix.Size; i++)
            {
                lines.MoveNext();
                using var values = GetParser(lines.Current);
                for (var j = 0; values.MoveNext(); j++)
                        matrix[i, j] = values.Current;
            }
            
            lines.MoveNext();
            using var values2 = GetParser(lines.Current);
            for (var j = 0; values2.MoveNext(); j++)
                matrix[j, matrix.Size] = values2.Current;
        }
        return matrix;
    }

    private IEnumerator<decimal> GetParser(string line)
    {
        return line
            .Split(" ")
            .Select(decimal.Parse)
            .GetEnumerator();
    }
}