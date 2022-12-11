using Lab6_NET.Models;

namespace Lab6_NET.Logic;


/// <summary>
/// Serializator pliku wejściowego z macierzą
/// </summary>
public static class Serializer
{
    /// <summary>
    /// Metoda wczytująca plik i zwracająca macierz
    /// </summary>
    /// <param name="file">plik wejściowy</param>
    /// <returns>macierz z wektorem</returns>
    /// <exception cref="Exception">błąd jeżeli plik nie istnieje</exception>
    public static Matrix2D SerializeMatrix(string file)
    {
        if (!File.Exists(file))
            throw new Exception("Podano Błędną ścieżkę do pliku!");

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
    
    
    private static IEnumerator<decimal> GetParser(string line)
    {
        return line
            .Split(" ")
            .Select(decimal.Parse)
            .GetEnumerator();
    }
}