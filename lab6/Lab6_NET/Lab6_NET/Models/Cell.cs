namespace Lab6_NET.Models;

public record struct Cell(int Row, int Col)
{ public override string ToString() => $"[{Row}, {Col}]"; }