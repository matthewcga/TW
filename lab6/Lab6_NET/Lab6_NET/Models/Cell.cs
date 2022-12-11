namespace Lab6_NET.Models;

public readonly record struct Cell(int Row, int Col)
{
    public static Cell Empty => new (0, 0);
    public override string ToString() => $"[{Row}, {Col}]";
}