using Lab6_NET.Enums;

namespace Lab6_NET.Models;

/// <summary>
/// Produkcja zawiera informacje o wykonanej operacji i komórkach na których została wykonana
/// </summary>
/// <param name="Operation">Operacja A, B lub C</param>
/// <param name="Pass">przejście algorytmu (informacja dla asynchronicznej implementacji)</param>
/// <param name="Cell1">komórka 1</param>
/// <param name="Cell2">komórka 2 (jeżeli operacja na dwóch komórkach)</param>
public record Production(EOperation Operation, int Pass, Cell Cell1, Cell Cell2)
{
    /// <summary>
    /// Sprawdza czy operacje są od siebie zależne
    /// </summary>
    /// <param name="other">sprawdzana operacja</param>
    /// <returns>czy operacje są zależne</returns>
    public bool IsDependentOn(Production other)
    {
        return Operation switch
        {
            EOperation.A => other.Operation switch
            {
                EOperation.A => Cell2.Row == other.Cell2.Row,
                EOperation.B => other.Pass < Pass,
                EOperation.C => other.Pass < Pass,
                _ => throw new ArgumentOutOfRangeException()
            },
            EOperation.B => other.Operation switch
            {
                EOperation.A => other.Pass <= Pass,
                EOperation.B => Cell1 == Cell2,
                EOperation.C => other.Pass < Pass,
                _ => throw new ArgumentOutOfRangeException()
            },
            EOperation.C => other.Operation switch
            {
                EOperation.A => other.Pass <= Pass,
                EOperation.B => other.Pass <= Pass,
                EOperation.C => Cell1 == Cell2,
                _ => throw new ArgumentOutOfRangeException()
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
        

    public override string ToString()
    { 
        return
            $"{Operation}" +
            $"({Cell1}" +
            $"{(Operation != EOperation.B ? $", {Cell2}" : string.Empty)})";
    }
}