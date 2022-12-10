using Lab6_NET.Enums;

namespace Lab6_NET.Models;

/// <summary>
/// Produkcja zawiera informacje o wykonanej operacji i komórkach na których została wykonana
/// </summary>
/// <param name="Operation">Operacja A, B lub C</param>
/// <param name="Cell1">komórka 1</param>
/// <param name="Cell2">komórka 2 (jeżeli operacja na dwóch komórkach)</param>
/// <param name="Pass">przejście algorytmu (informacja dla anychronicznej implementacji)</param>
public record Production(EOperation Operation, Cell Cell1, Cell? Cell2 = null, int? Pass = null)
{
    /// <summary>
    /// Sprawdza czy operacje są od siebie zależne
    /// </summary>
    /// <param name="other">sprawdzana operacja</param>
    /// <returns>czy operacje są zależne</returns>
    public bool CheckDependency(Production other)
    {
        /* TODO fix
        if (Operation == EOperation.A && other.Operation == EOperation.A)
            return false;

        return
            Cell1 == other.Cell1 || Cell1 == other.Cell2 || Cell2 == other.Cell1;
        */

        return
            Cell1 == other.Cell1 || Cell1 == other.Cell2 ||
            Cell2 == other.Cell1 || Cell2 == other.Cell2;
    }
        

    public override string ToString() =>
        $"{Operation}" +
        $"({Cell1}" +
        $"{(Cell2.HasValue ? $", {Cell2}" : string.Empty)})";
}