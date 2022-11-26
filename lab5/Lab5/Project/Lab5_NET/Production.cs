namespace Lab5_NET;

/// <summary>
/// Rekord odzwierciedlający produkcje v0 = v1 @ v2.
/// </summary>
public readonly record struct Production(char V0, char? V1, char? V2)
{
    /// <summary>
    /// Sprawdza czy relacja jest zależna od drugiej
    /// </summary>
    /// <param name="other">druga produkcja</param>
    /// <returns>prawda jeżeli produkcje są zależne</returns>
    public bool CheckDependency(Production other)
    {
        if (V0 == other.V1 || V0 == other.V2) return true; // a modyfikuje parametry b
        if (other.V0 == V1 || other.V0 == V2) return true; // b modyfikuje parametry a
        return V0 == other.V0;                             // a i b modyfikują tą samą zmienną
    }

    public override string ToString() =>
        $"{V0} = {V1}{(V1.HasValue && V2.HasValue ? "@" : string.Empty)}{V2}";
}